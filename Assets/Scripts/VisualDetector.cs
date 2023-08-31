using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;


public class VisualDetector : MonoBehaviour
{
    [SerializeField]
    float fovWidth = 7f;

    [SerializeField]
    float fovHeight = 5f;

    [SerializeField]
    float fovDepth = 7f;

    private MeshCollider meshCollider;

    public void OnEnable()
    {
        meshCollider = GetComponent<MeshCollider>();
        var filter = GetComponent<MeshFilter>();
        filter.sharedMesh = MeshBuilder.BuildPyramid(fovWidth, fovDepth, fovHeight);
        filter.transform.rotation = Quaternion.Euler(-90, 0, 0);
        meshCollider.sharedMesh = filter.sharedMesh;
    }

    public void OnTriggerEnter(Collider other)
    {
        var direction = other.transform.position - transform.position;
        direction = direction.normalized;

        var ray = new Ray(transform.position, direction);
        var range = fovDepth;

        if (Physics.Raycast(ray, out var hit, range))
        {
            if (hit.collider.TryGetComponent<VisualNotifier>(out var notifier))
            {
                notifier.Spotted(this);
            }
        }
    }
    
    Vector3 DirectionFrom(float radians)
    {
        var direction = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians));
        return direction.normalized;
    }
}

/*
date()
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
}*/