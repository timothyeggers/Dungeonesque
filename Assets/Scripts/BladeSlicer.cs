using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;


public class BladeSlicer: MonoBehaviour, ITrigger
{
    [SerializeField] private Transform bladeEdgeForward;

    public List<OnTriggerEntered> onTriggerEntered { get; } = new List<OnTriggerEntered>();

    public List<OnTriggerExited> onTriggerExited { get; } = new List<OnTriggerExited>();

    public void RegisterListener(OnTriggerEntered onTriggerEntered, OnTriggerExited onTriggerExited)
    {
        this.onTriggerEntered.Add(onTriggerEntered);
        this.onTriggerExited.Add(onTriggerExited);
    }

    // blade contact
    public void OnTriggerEnter(Collider other)
    {
        onTriggerEntered.ForEach(x => x.Invoke(other));

        SliceInstantiate(other.gameObject, other.ClosestPoint(transform.position), bladeEdgeForward.transform.up);
    }

    public GameObject[] SliceInstantiate(GameObject objectToSlice, Vector3 planeWorldPosition, Vector3 planeWorldDirection)
    {
        var objs = objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection);
        objectToSlice.SetActive(false);
        foreach (var obj in objs)
        {
            var coll = obj.AddComponent<MeshCollider>();
            coll.convex = true;
            obj.AddComponent<Rigidbody>();

        }
        return objs;
    }
}
