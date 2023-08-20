using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class VisualNotifier : MonoBehaviour
{
    [SerializeField]
    GameEvent onDistractionSpotted;

    [SerializeField]
    GameEvent onPlayerSpotted;

    [SerializeField]
    GameEvent onEnemySpotted;

    [SerializeField]
    float weight = 1f;

    public void Spotted(Component from, RaycastHit hit)
    {
        onDistractionSpotted?.Raise(this, hit);
    }
}