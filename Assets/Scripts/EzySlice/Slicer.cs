using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace EzySlice
{
    /**
     * Contains methods for slicing GameObjects
     */
    public sealed class Slicer
    {
        /**
         * Helper function to accept a gameobject which will transform the plane
         * approprietly before the slice occurs
         * See -> Slice(Mesh, Plane) for more info
         */
        public static SlicedHull Slice(GameObject obj, Plane pl, TextureRegion crossRegion, Material crossMaterial)
        {
            // cannot continue without a proper filter
            if (!obj.TryGetComponent<MeshFilter>(out var filter))
            {
                Debug.LogWarning("EzySlice::Slice -> Provided GameObject must have a MeshFilter Component.");

                return null;
            }


            // cannot continue without a proper renderer
            if (!obj.TryGetComponent<MeshRenderer>(out var renderer))
            {
                Debug.LogWarning("EzySlice::Slice -> Provided GameObject must have a MeshRenderer Component.");

                return null;
            }

            var materials = renderer.sharedMaterials;

            var mesh = filter.sharedMesh;

            // cannot slice a mesh that doesn't exist
            if (mesh == null)
            {
                Debug.LogWarning("EzySlice::Slice -> Provided GameObject must have a Mesh that is not NULL.");

                return null;
            }

            var submeshCount = mesh.subMeshCount;

            // to make things straightforward, exit without slicing if the materials and mesh
            // array don't match. This shouldn't happen anyway
            if (materials.Length != submeshCount)
            {
                Debug.LogWarning("EzySlice::Slice -> Provided Material array must match the length of submeshes.");

                return null;
            }

            // we need to find the index of the material for the cross section.
            // default to the end of the array
            var crossIndex = materials.Length;

            // for cases where the sliced material is null, we will append the cross section to the end
            // of the submesh array, this is because the application may want to set/change the material
            // after slicing has occured, so we don't assume anything
            if (crossMaterial != null)
                for (var i = 0; i < crossIndex; i++)
                    if (materials[i] == crossMaterial)
                    {
                        crossIndex = i;
                        break;
                    }

            return Slice(mesh, pl, crossRegion, crossIndex);
        }

        /**
         * Slice the gameobject mesh (if any) using the Plane, which will generate
         * a maximum of 2 other Meshes.
         * This function will recalculate new UV coordinates to ensure textures are applied
         * properly.
         * Returns null if no intersection has been found or the GameObject does not contain
         * a valid mesh to cut.
         */
        public static SlicedHull Slice(Mesh sharedMesh, Plane pl, TextureRegion region, int crossIndex)
        {
            if (sharedMesh == null) return null;

            var verts = sharedMesh.vertices;
            var uv = sharedMesh.uv;
            var norm = sharedMesh.normals;
            var tan = sharedMesh.tangents;

            var submeshCount = sharedMesh.subMeshCount;

            // each submesh will be sliced and placed in its own array structure
            var slices = new SlicedSubmesh[submeshCount];
            // the cross section hull is common across all submeshes
            var crossHull = new List<Vector3>();

            // we reuse this object for all intersection tests
            var result = new IntersectionResult();

            // see if we would like to split the mesh using uv, normals and tangents
            var genUV = verts.Length == uv.Length;
            var genNorm = verts.Length == norm.Length;
            var genTan = verts.Length == tan.Length;

            // iterate over all the submeshes individually. vertices and indices
            // are all shared within the submesh
            for (var submesh = 0; submesh < submeshCount; submesh++)
            {
                var indices = sharedMesh.GetTriangles(submesh);
                var indicesCount = indices.Length;

                var mesh = new SlicedSubmesh();

                // loop through all the mesh vertices, generating upper and lower hulls
                // and all intersection points
                for (var index = 0; index < indicesCount; index += 3)
                {
                    var i0 = indices[index + 0];
                    var i1 = indices[index + 1];
                    var i2 = indices[index + 2];

                    var newTri = new Triangle(verts[i0], verts[i1], verts[i2]);

                    // generate UV if available
                    if (genUV) newTri.SetUV(uv[i0], uv[i1], uv[i2]);

                    // generate normals if available
                    if (genNorm) newTri.SetNormal(norm[i0], norm[i1], norm[i2]);

                    // generate tangents if available
                    if (genTan) newTri.SetTangent(tan[i0], tan[i1], tan[i2]);

                    // slice this particular triangle with the provided
                    // plane
                    if (newTri.Split(pl, result))
                    {
                        var upperHullCount = result.upperHullCount;
                        var lowerHullCount = result.lowerHullCount;
                        var interHullCount = result.intersectionPointCount;

                        for (var i = 0; i < upperHullCount; i++) mesh.upperHull.Add(result.upperHull[i]);

                        for (var i = 0; i < lowerHullCount; i++) mesh.lowerHull.Add(result.lowerHull[i]);

                        for (var i = 0; i < interHullCount; i++) crossHull.Add(result.intersectionPoints[i]);
                    }
                    else
                    {
                        var sa = pl.SideOf(verts[i0]);
                        var sb = pl.SideOf(verts[i1]);
                        var sc = pl.SideOf(verts[i2]);

                        var side = SideOfPlane.ON;
                        if (sa != SideOfPlane.ON) side = sa;

                        if (sb != SideOfPlane.ON)
                        {
                            Debug.Assert(side == SideOfPlane.ON || side == sb);
                            side = sb;
                        }

                        if (sc != SideOfPlane.ON)
                        {
                            Debug.Assert(side == SideOfPlane.ON || side == sc);
                            side = sc;
                        }

                        if (side == SideOfPlane.UP || side == SideOfPlane.ON)
                            mesh.upperHull.Add(newTri);
                        else
                            mesh.lowerHull.Add(newTri);
                    }
                }

                // register into the index
                slices[submesh] = mesh;
            }

            // check if slicing actually occured
            for (var i = 0; i < slices.Length; i++)
                // check if at least one of the submeshes was sliced. If so, stop checking
                // because we need to go through the generation step
                if (slices[i] != null && slices[i].isValid)
                    return CreateFrom(slices, CreateFrom(crossHull, pl.normal, region), crossIndex);

            // no slicing occured, just return null to signify
            return null;
        }

        /**
         * Generates a single SlicedHull from a set of cut submeshes
         */
        private static SlicedHull CreateFrom(SlicedSubmesh[] meshes, List<Triangle> cross, int crossSectionIndex)
        {
            var submeshCount = meshes.Length;

            var upperHullCount = 0;
            var lowerHullCount = 0;

            // get the total amount of upper, lower and intersection counts
            for (var submesh = 0; submesh < submeshCount; submesh++)
            {
                upperHullCount += meshes[submesh].upperHull.Count;
                lowerHullCount += meshes[submesh].lowerHull.Count;
            }

            var upperHull = CreateUpperHull(meshes, upperHullCount, cross, crossSectionIndex);
            var lowerHull = CreateLowerHull(meshes, lowerHullCount, cross, crossSectionIndex);

            return new SlicedHull(upperHull, lowerHull);
        }

        private static Mesh CreateUpperHull(SlicedSubmesh[] mesh, int total, List<Triangle> crossSection,
            int crossSectionIndex)
        {
            return CreateHull(mesh, total, crossSection, crossSectionIndex, true);
        }

        private static Mesh CreateLowerHull(SlicedSubmesh[] mesh, int total, List<Triangle> crossSection,
            int crossSectionIndex)
        {
            return CreateHull(mesh, total, crossSection, crossSectionIndex, false);
        }

        /**
         * Generate a single Mesh HULL of either the UPPER or LOWER hulls.
         */
        private static Mesh CreateHull(SlicedSubmesh[] meshes, int total, List<Triangle> crossSection, int crossIndex,
            bool isUpper)
        {
            if (total <= 0) return null;

            var submeshCount = meshes.Length;
            var crossCount = crossSection != null ? crossSection.Count : 0;

            var newMesh = new Mesh();
            newMesh.indexFormat = IndexFormat.UInt32;

            var arrayLen = (total + crossCount) * 3;

            var hasUV = meshes[0].hasUV;
            var hasNormal = meshes[0].hasNormal;
            var hasTangent = meshes[0].hasTangent;

            // vertices and uv's are common for all submeshes
            var newVertices = new Vector3[arrayLen];
            var newUvs = hasUV ? new Vector2[arrayLen] : null;
            var newNormals = hasNormal ? new Vector3[arrayLen] : null;
            var newTangents = hasTangent ? new Vector4[arrayLen] : null;

            // each index refers to our submesh triangles
            var triangles = new List<int[]>(submeshCount);

            var vIndex = 0;

            // first we generate all our vertices, uv's and triangles
            for (var submesh = 0; submesh < submeshCount; submesh++)
            {
                // pick the hull we will be playing around with
                var hull = isUpper ? meshes[submesh].upperHull : meshes[submesh].lowerHull;
                var hullCount = hull.Count;

                var indices = new int[hullCount * 3];

                // fill our mesh arrays
                for (int i = 0, triIndex = 0; i < hullCount; i++, triIndex += 3)
                {
                    var newTri = hull[i];

                    var i0 = vIndex + 0;
                    var i1 = vIndex + 1;
                    var i2 = vIndex + 2;

                    // add the vertices
                    newVertices[i0] = newTri.positionA;
                    newVertices[i1] = newTri.positionB;
                    newVertices[i2] = newTri.positionC;

                    // add the UV coordinates if any
                    if (hasUV)
                    {
                        newUvs[i0] = newTri.uvA;
                        newUvs[i1] = newTri.uvB;
                        newUvs[i2] = newTri.uvC;
                    }

                    // add the Normals if any
                    if (hasNormal)
                    {
                        newNormals[i0] = newTri.normalA;
                        newNormals[i1] = newTri.normalB;
                        newNormals[i2] = newTri.normalC;
                    }

                    // add the Tangents if any
                    if (hasTangent)
                    {
                        newTangents[i0] = newTri.tangentA;
                        newTangents[i1] = newTri.tangentB;
                        newTangents[i2] = newTri.tangentC;
                    }

                    // triangles are returned in clocwise order from the
                    // intersector, no need to sort these
                    indices[triIndex] = i0;
                    indices[triIndex + 1] = i1;
                    indices[triIndex + 2] = i2;

                    vIndex += 3;
                }

                // add triangles to the index for later generation
                triangles.Add(indices);
            }

            // generate the cross section required for this particular hull
            if (crossSection != null && crossCount > 0)
            {
                var crossIndices = new int[crossCount * 3];

                for (int i = 0, triIndex = 0; i < crossCount; i++, triIndex += 3)
                {
                    var newTri = crossSection[i];

                    var i0 = vIndex + 0;
                    var i1 = vIndex + 1;
                    var i2 = vIndex + 2;

                    // add the vertices
                    newVertices[i0] = newTri.positionA;
                    newVertices[i1] = newTri.positionB;
                    newVertices[i2] = newTri.positionC;

                    // add the UV coordinates if any
                    if (hasUV)
                    {
                        newUvs[i0] = newTri.uvA;
                        newUvs[i1] = newTri.uvB;
                        newUvs[i2] = newTri.uvC;
                    }

                    // add the Normals if any
                    if (hasNormal)
                    {
                        // invert the normals dependiong on upper or lower hull
                        if (isUpper)
                        {
                            newNormals[i0] = -newTri.normalA;
                            newNormals[i1] = -newTri.normalB;
                            newNormals[i2] = -newTri.normalC;
                        }
                        else
                        {
                            newNormals[i0] = newTri.normalA;
                            newNormals[i1] = newTri.normalB;
                            newNormals[i2] = newTri.normalC;
                        }
                    }

                    // add the Tangents if any
                    if (hasTangent)
                    {
                        newTangents[i0] = newTri.tangentA;
                        newTangents[i1] = newTri.tangentB;
                        newTangents[i2] = newTri.tangentC;
                    }

                    // add triangles in clockwise for upper
                    // and reversed for lower hulls, to ensure the mesh
                    // is facing the right direction
                    if (isUpper)
                    {
                        crossIndices[triIndex] = i0;
                        crossIndices[triIndex + 1] = i1;
                        crossIndices[triIndex + 2] = i2;
                    }
                    else
                    {
                        crossIndices[triIndex] = i0;
                        crossIndices[triIndex + 1] = i2;
                        crossIndices[triIndex + 2] = i1;
                    }

                    vIndex += 3;
                }

                // add triangles to the index for later generation
                if (triangles.Count <= crossIndex)
                {
                    triangles.Add(crossIndices);
                }
                else
                {
                    // otherwise, we need to merge the triangles for the provided subsection
                    var prevTriangles = triangles[crossIndex];
                    var merged = new int[prevTriangles.Length + crossIndices.Length];

                    Array.Copy(prevTriangles, merged, prevTriangles.Length);
                    Array.Copy(crossIndices, 0, merged, prevTriangles.Length, crossIndices.Length);

                    // replace the previous array with the new merged array
                    triangles[crossIndex] = merged;
                }
            }

            var totalTriangles = triangles.Count;

            newMesh.subMeshCount = totalTriangles;
            // fill the mesh structure
            newMesh.vertices = newVertices;

            if (hasUV) newMesh.uv = newUvs;

            if (hasNormal) newMesh.normals = newNormals;

            if (hasTangent) newMesh.tangents = newTangents;

            // add the submeshes
            for (var i = 0; i < totalTriangles; i++) newMesh.SetTriangles(triangles[i], i, false);

            return newMesh;
        }

        /**
         * Generate Two Meshes (an upper and lower) cross section from a set of intersection
         * points and a plane normal. Intersection Points do not have to be in order.
         */
        private static List<Triangle> CreateFrom(List<Vector3> intPoints, Vector3 planeNormal, TextureRegion region)
        {
            return Triangulator.MonotoneChain(intPoints, planeNormal, out var tris, region) ? tris : null;
        }

        /**
         * An internal class for storing internal submesh values
         */
        internal class SlicedSubmesh
        {
            public readonly List<Triangle> lowerHull = new();
            public readonly List<Triangle> upperHull = new();

            /**
             * Check if the submesh has had any UV's added.
             * NOTE -> This should be supported properly
             */
            public bool hasUV =>
                // what is this abomination??
                upperHull.Count > 0 ? upperHull[0].hasUV : lowerHull.Count > 0 && lowerHull[0].hasUV;

            /**
             * Check if the submesh has had any Normals added.
             * NOTE -> This should be supported properly
             */
            public bool hasNormal =>
                // what is this abomination??
                upperHull.Count > 0 ? upperHull[0].hasNormal : lowerHull.Count > 0 && lowerHull[0].hasNormal;

            /**
             * Check if the submesh has had any Tangents added.
             * NOTE -> This should be supported properly
             */
            public bool hasTangent =>
                // what is this abomination??
                upperHull.Count > 0 ? upperHull[0].hasTangent : lowerHull.Count > 0 && lowerHull[0].hasTangent;

            /**
             * Check if proper slicing has occured for this submesh. Slice occured if there
             * are triangles in both the upper and lower hulls
             */
            public bool isValid => upperHull.Count > 0 && lowerHull.Count > 0;
        }
    }
}