using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


[CreateAssetMenu(fileName = "Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField]
    List<WeaponSO> weapons = new List<WeaponSO>();

    [NonSerialized]
    WeaponSO empty;

    private void OnEnable()
    {
        empty = CreateInstance<WeaponSO>();
    }

    public WeaponSO Get(int index)
    {
        if (index == 0) return empty;
        if (index > weapons.Count) return empty;
        return weapons[index-1];
    }
}