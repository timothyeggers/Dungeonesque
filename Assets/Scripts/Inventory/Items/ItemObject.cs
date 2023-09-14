using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    NonSelectable
}

public class ItemObject : ScriptableObject
{
    /* 
     * The objective of ItemObject is to contain item data.
     * It's exclusively for inventory management - equipping and unequipping, and
     * holding it's item data.
     */
    public GameObject prefab;
    public ItemType type = ItemType.Default;
    public string description;
    public float stowTime;
    public float unstowTime;
}
