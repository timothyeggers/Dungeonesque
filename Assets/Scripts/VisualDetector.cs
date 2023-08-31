using System;
using System.Collections.Generic;
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
        if (other.TryGetComponent<VisualNotifier>(out var notifier))
        {
            notifier.Spotted(this);
        }
    }
    
    Vector3 DirectionFrom(float radians)
    {
        var direction = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians));
        return direction.normalized;
    }
}