using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;


[ExecuteInEditMode]
[RequireComponent(typeof(MeshCollider), typeof(MeshFilter))]
public class VisualDetector : MonoBehaviour, ITrigger
{
    public List<OnTriggerEntered> onTriggerEntered { get; private set; } = new List<OnTriggerEntered>();
    public List<OnTriggerExited> onTriggerExited { get; private set; } = new List<OnTriggerExited>();

    [SerializeField] LayerMask opaqueLayers;

    [SerializeField] float fovWidth = 7f;
    [SerializeField] float fovHeight = 5f;
    [SerializeField] float fovDepth = 13f;

    private MeshCollider meshCollider;

    public void OnEnable()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.isTrigger = true;

        var filter = GetComponent<MeshFilter>();
        filter.sharedMesh = MeshBuilder.BuildPyramid(fovWidth, fovDepth-1, fovHeight);
        filter.transform.rotation = Quaternion.Euler(-90, 0, 0);

        meshCollider.sharedMesh = filter.sharedMesh;
    }

    public void OnTriggerEnter(Collider other)
    {
        var direction = other.transform.position - transform.position;
        direction = direction.normalized;

        var ray = new Ray(transform.position, direction);
        var range = fovDepth;

        if (Physics.Raycast(ray, out var hit, range, opaqueLayers))
        {
            onTriggerEntered.ForEach(x => x.Invoke(other));
        }
    }

    public void OnTriggerExit(Collider other)
    {
        onTriggerExited.ForEach(x => x.Invoke(other));
    }
    
    public void RegisterListener(OnTriggerEntered onTriggerEntered, OnTriggerExited onTriggerExited)
    {
        if (!this.onTriggerEntered.Contains(onTriggerEntered)) this.onTriggerEntered.Add(onTriggerEntered);
        if (!this.onTriggerExited.Contains(onTriggerExited)) this.onTriggerExited.Add(onTriggerExited);
    }

    public void RemoveListener(OnTriggerEntered onTriggerEntered, OnTriggerExited onTriggerExited)
    {
        if (this.onTriggerEntered.Contains(onTriggerEntered)) this.onTriggerEntered.Remove(onTriggerEntered);
        if (this.onTriggerExited.Contains(onTriggerExited)) this.onTriggerExited.Remove(onTriggerExited);
    }
}
