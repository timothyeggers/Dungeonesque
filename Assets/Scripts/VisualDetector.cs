using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;


public delegate void OnSeen(Component sender);
public delegate void OnSeenFully(GameObject target);

public class VisualDetector : MonoBehaviour
{
    [SerializeField]
    LayerMask opaqueLayers;

    [SerializeField]
    float fovWidth = 7f;

    [SerializeField]
    float fovHeight = 5f;

    [SerializeField]
    float fovDepth = 13f;

    [SerializeField] float minDistanceUntilFullySpotted = 7f;
    [SerializeField] float timeUntilFullySpotted = 1.4f;

    GameObject target;
    float targetInRangeDt = 0f;

    private MeshCollider meshCollider;

    private List<OnSeen> onSeenCallbacks = new List<OnSeen>();
    private List<OnSeenFully> onSeenFullyCallbacks = new List<OnSeenFully>();

    public void OnEnable()
    {
        meshCollider = GetComponent<MeshCollider>();
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
            if (hit.collider.TryGetComponent<VisualNotifier>(out var notifier))
            {
                notifier.Spotted(this);
                onSeenCallbacks.ForEach(x => x.Invoke(notifier));
                target = notifier.gameObject;
            }
        }
    }

    private void Update()
    {
        if (target != null)
        {
            targetInRangeDt += Time.deltaTime;
            if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= minDistanceUntilFullySpotted)
            {
                targetInRangeDt = timeUntilFullySpotted;
            }
            if (Vector3.Distance(gameObject.transform.position, target.transform.position) >= fovDepth + 2)
            {
                target = null;
            }
        }

        if (targetInRangeDt >= timeUntilFullySpotted)
        {
            onSeenFullyCallbacks.ForEach(x => x.Invoke(target));
            targetInRangeDt = 0;
            target = null;
        }
    }

    public void RegisterListener(OnSeen onSeen, OnSeenFully onSeenFully)
    {
        onSeenCallbacks.Add(onSeen);
        onSeenFullyCallbacks.Add(onSeenFully);
    }
}
