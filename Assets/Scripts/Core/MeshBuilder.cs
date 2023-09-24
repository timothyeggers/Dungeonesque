using UnityEngine;

namespace Dungeonesque.Core
{
    public static class MeshBuilder
    {
        public static Mesh BuildPyramid(float width = 2f, float height = 2f, float depth = 2f, int sideCount = 4)
        {
            var mesh = new Mesh();

            var size = sideCount;
            var vertextCount = size + 2;
            var triangleCount = size * 2;

            var vertices = new Vector3[vertextCount];
            var triangles = new int[triangleCount * 3];

            var alpha = Mathf.PI * 2 / size;
            var omega = alpha * 0.5f;
            for (var i = 0; i < size; i++)
            {
                vertices[i + 2] = new Vector3(Mathf.Cos(i * alpha + omega) * width, -height,
                    Mathf.Sin(i * alpha + omega) * depth);
                var oldIndex = i == 0 ? vertextCount - 1 : i + 1;
                var offset = i * 6;
                triangles[offset + 0] = i + 2;
                triangles[offset + 1] = oldIndex;
                triangles[offset + 2] = 0;
                triangles[offset + 3] = i + 2;
                triangles[offset + 4] = 1;
                triangles[offset + 5] = oldIndex;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}