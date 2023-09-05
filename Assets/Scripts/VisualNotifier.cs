using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class VisualNotifier : MonoBehaviour, IPriority
{
    [SerializeField]
    float visibility = 1f;

    #region IPriority Implementation
    public int UUID => gameObject.GetInstanceID();
    public float weight => visibility;
    public Priorities type => Priorities.Visual;
    #endregion

    public void Spotted(Component from)
    {
        Debug.Log($"you've been spotted from {from}.");
    }
}