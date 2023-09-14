using System;

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;


public delegate void OnHeard(Collider sender);

public class AudioDetector : MonoBehaviour
{
    [SerializeField]
    LayerMask audioOpaqueLayers;

    [SerializeField]
    float maxAudibleRange;
    
    private List<OnHeard> onHeardCallbacks = new List<OnHeard>();

    // TODO: ask this to pass in AudioTarget serialized data. instead of using old AudioTriggerComponetn with parameter search
    public void AudioTriggered(Component sender)
    {
        var path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, sender.transform.position, audioOpaqueLayers, path))
        {
            var distance = GetPathLength(path);
            /*var extraRange = sender.TryGetComponent<AudioTrigger>(out var trigger) ? trigger.volume : 0;*/
            if (distance < maxAudibleRange) // + extraRange
            {
                onHeardCallbacks.ForEach(x => x.Invoke(null));
            }
        }
    }

    public void RegisterListener(OnHeard callback)
    {
        onHeardCallbacks.Add(callback);
    }


    public static float GetPathLength(NavMeshPath path)
    {
        float lng = 0.0f;

        if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
        {
            for (int i = 1; i < path.corners.Length; ++i)
            {
                lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }

        return lng;
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