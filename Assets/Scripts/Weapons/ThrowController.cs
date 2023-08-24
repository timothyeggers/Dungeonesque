using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    [SerializeField] private WeaponStatsObject weaponStats;

    public void Awake()
    {
        Debug.Log("Spawned a throwable.");
    }

    public void Throw()
    {
        Debug.Log($"Will throw object with {weaponStats} stats.");
    }
}