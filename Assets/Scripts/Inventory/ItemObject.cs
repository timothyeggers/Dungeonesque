using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    Weapon
}

public class ItemObject : ScriptableObject
{
    public GameObject prefab;
    [NonSerialized] public bool prefabInScene = false;
    public ItemType type;
    public string description;
    public float stowTime;
    public float unstowTime;
}