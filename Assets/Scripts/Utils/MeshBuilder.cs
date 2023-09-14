using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MeshBuilder
{
    public static Mesh BuildPyramid(float width = 2f, float height = 2f, float depth = 2f, int sideCount = 4)
    {
        var mesh = new Mesh();

        int size = sideCount;
        int vertextCount = size + 2;
        int triangleCount = size * 2;

        Vector3[] vertices = new Vector3[vertextCount];
        int[] triangles = new int[triangleCount * 3];

        float alpha = Mathf.PI * 2 / size;
        float omega = alpha * 0.5f;
        for (int i = 0; i < size; i++)
        {
            vertices[i + 2] = new Vector3(Mathf.Cos(i * alpha + omega) * width, -height, Mathf.Sin(i * alpha + omega) * depth);
            int oldIndex = i == 0 ? vertextCount - 1 : i + 1;
            int offset = i * 6;
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
