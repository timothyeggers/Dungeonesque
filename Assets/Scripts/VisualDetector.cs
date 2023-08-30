using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;


[ExecuteInEditMode]
public class VisualDetector : MonoBehaviour
{
    [SerializeField]
    LayerMask monitorLayer;

    [SerializeField]
    float FOV = 90f;

    [SerializeField]
    int resolution = 10;

    [SerializeField]
    float range = 10f;

    [SerializeField]
    Transform eyeLocation;

    [SerializeField]
    float fovWidth = 3f;

    [SerializeField]
    float fovHeight = 2f;

    [SerializeField]
    float fovDepth = 6f;

    public void Start()
    {
        var meshObject = Instantiate(new GameObject("FOV Mesh"), eyeLocation);
        var filter = meshObject.AddComponent<MeshFilter>();
        meshObject.AddComponent<MeshRenderer>();
        filter.mesh = MeshBuilder.BuildPyramid(fovWidth, fovDepth, fovHeight);
        meshObject.transform.rotation = Quaternion.Euler(-90, 0, 0);       
    }

    private void Update()
    {
        float fovRads = FOV * Mathf.Deg2Rad;
        fovRads /= resolution;

        for (int i = -(resolution / 2); i <= (resolution / 2); i++)
        {
            float angle = fovRads * i;
            var direction = DirectionFrom(angle);
            direction = Quaternion.Euler(0, transform.eulerAngles.y, 0) * direction;

            var ray = new Ray(transform.position, direction);
            var range = this.range;

            if (Physics.Raycast(ray, out var hit, range, monitorLayer))
            {
                if (hit.collider.TryGetComponent<VisualNotifier>(out var notifier))
                {
                    notifier.Spotted(this, hit);
                }
                range = Vector3.Distance(ray.origin, hit.point);
            }

            Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
        }
    }

    Vector3 DirectionFrom(float radians)
    {
        var direction = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians));
        return direction.normalized;
    }

}