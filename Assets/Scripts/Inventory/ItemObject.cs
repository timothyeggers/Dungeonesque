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
    public ItemType type;
    public string description;
}