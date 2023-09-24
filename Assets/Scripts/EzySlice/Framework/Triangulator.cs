using System;
using System.Collections.Generic;
using UnityEngine;

namespace EzySlice
{
    /**
     * Contains static functionality for performing Triangulation on arbitrary vertices.
     * Read the individual function descriptions for specific details.
     */
    public sealed class Triangulator
    {
        /**
         * Overloaded variant of MonotoneChain which will calculate UV coordinates of the Triangles
         * between 0.0 and 1.0 (default).
         * 
         * See MonotoneChain(vertices, normal, tri, TextureRegion) for full explanation
         */
        public static bool MonotoneChain(List<Vector3> vertices, Vector3 normal, out List<Triangle> tri)
        {
            // default texture region is in coordinates 0,0 to 1,1
            return MonotoneChain(vertices, normal, out tri, new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f));
        }

        /**
         * O(n log n) Convex Hull Algorithm.
         * Accepts a list of vertices as Vector3 and triangulates them according to a projection
         * plane defined as planeNormal. Algorithm will output vertices, indices and UV coordinates
         * as arrays
         */
        public static bool MonotoneChain(List<Vector3> vertices, Vector3 normal, out List<Triangle> tri,
            TextureRegion texRegion)
        {
            var count = vertices.Count;

            // we cannot triangulate less than 3 points. Use minimum of 3 points
            if (count < 3)
            {
                tri = null;
                return false;
            }

            // first, we map from 3D points into a 2D plane represented by the provided normal
            var u = Vector3.Normalize(Vector3.Cross(normal, Vector3.up));
            if (Vector3.zero == u) u = Vector3.Normalize(Vector3.Cross(normal, Vector3.forward));
            var v = Vector3.Cross(u, normal);

            // generate an array of mapped values
            var mapped = new Mapped2D[count];

            // these values will be used to generate new UV coordinates later on
            var maxDivX = float.MinValue;
            var maxDivY = float.MinValue;
            var minDivX = float.MaxValue;
            var minDivY = float.MaxValue;

            // map the 3D vertices into the 2D mapped values
            for (var i = 0; i < count; i++)
            {
                var vertToAdd = vertices[i];

                var newMappedValue = new Mapped2D(vertToAdd, u, v);
                var mapVal = newMappedValue.mappedValue;

                // grab our maximal values so we can map UV's in a proper range
                maxDivX = Mathf.Max(maxDivX, mapVal.x);
                maxDivY = Mathf.Max(maxDivY, mapVal.y);
                minDivX = Mathf.Min(minDivX, mapVal.x);
                minDivY = Mathf.Min(minDivY, mapVal.y);

                mapped[i] = newMappedValue;
            }

            // sort our newly generated array values
            Array.Sort(mapped, (a, b) =>
            {
                var x = a.mappedValue;
                var p = b.mappedValue;

                return x.x < p.x || (x.x == p.x && x.y < p.y) ? -1 : 1;
            });

            // our final hull mappings will end up in here
            var hulls = new Mapped2D[count + 1];

            var k = 0;

            // build the lower hull of the chain
            for (var i = 0; i < count; i++)
            {
                while (k >= 2)
                {
                    var mA = hulls[k - 2].mappedValue;
                    var mB = hulls[k - 1].mappedValue;
                    var mC = mapped[i].mappedValue;

                    if (Intersector.TriArea2D(mA.x, mA.y, mB.x, mB.y, mC.x, mC.y) > 0.0f) break;

                    k--;
                }

                hulls[k++] = mapped[i];
            }

            // build the upper hull of the chain
            for (int i = count - 2, t = k + 1; i >= 0; i--)
            {
                while (k >= t)
                {
                    var mA = hulls[k - 2].mappedValue;
                    var mB = hulls[k - 1].mappedValue;
                    var mC = mapped[i].mappedValue;

                    if (Intersector.TriArea2D(mA.x, mA.y, mB.x, mB.y, mC.x, mC.y) > 0.0f) break;

                    k--;
                }

                hulls[k++] = mapped[i];
            }

            // finally we can build our mesh, generate all the variables
            // and fill them up
            var vertCount = k - 1;
            var triCount = (vertCount - 2) * 3;

            // this should not happen, but here just in case
            if (vertCount < 3)
            {
                tri = null;
                return false;
            }

            // ensure List does not dynamically grow, performing copy ops each time!
            tri = new List<Triangle>(triCount / 3);

            var width = maxDivX - minDivX;
            var height = maxDivY - minDivY;

            var indexCount = 1;

            // generate both the vertices and uv's in this loop
            for (var i = 0; i < triCount; i += 3)
            {
                // the Vertices in our triangle
                var posA = hulls[0];
                var posB = hulls[indexCount];
                var posC = hulls[indexCount + 1];

                // generate UV Maps
                var uvA = posA.mappedValue;
                var uvB = posB.mappedValue;
                var uvC = posC.mappedValue;

                uvA.x = (uvA.x - minDivX) / width;
                uvA.y = (uvA.y - minDivY) / height;

                uvB.x = (uvB.x - minDivX) / width;
                uvB.y = (uvB.y - minDivY) / height;

                uvC.x = (uvC.x - minDivX) / width;
                uvC.y = (uvC.y - minDivY) / height;

                var newTriangle = new Triangle(posA.originalValue, posB.originalValue, posC.originalValue);

                // ensure our UV coordinates are mapped into the requested TextureRegion
                newTriangle.SetUV(texRegion.Map(uvA), texRegion.Map(uvB), texRegion.Map(uvC));

                // the normals is the same for all vertices since the final mesh is completly flat
                newTriangle.SetNormal(normal, normal, normal);
                newTriangle.ComputeTangents();

                tri.Add(newTriangle);

                indexCount++;
            }

            return true;
        }

        /**
         * Represents a 3D Vertex which has been mapped onto a 2D surface
         * and is mainly used in MonotoneChain to triangulate a set of vertices
         * against a flat plane.
         */
        internal struct Mapped2D
        {
            public Mapped2D(Vector3 newOriginal, Vector3 u, Vector3 v)
            {
                originalValue = newOriginal;
                mappedValue = new Vector2(Vector3.Dot(newOriginal, u), Vector3.Dot(newOriginal, v));
            }

            public Vector2 mappedValue { get; }

            public Vector3 originalValue { get; }
        }
    }
}