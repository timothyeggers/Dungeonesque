using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class VisualNotifier : MonoBehaviour
{
    [SerializeField]
    GameEvent onVisualDetectorEntered;

    [SerializeField]
    float weight = 1f;

    public void Spotted(Component from, RaycastHit hit)
    {
        onVisualDetectorEntered?.Raise(this, hit);
    }
}