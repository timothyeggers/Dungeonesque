using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


[CreateAssetMenu(fileName = "Inventory Object")]
public class InventoryObject : ScriptableObject
{
    [SerializeField]
    private List<ItemObject> items = new List<ItemObject>();    

    public void Add(ItemObject item)
    {
        items.Add(item);
    }

    public List<ItemObject> GetItems(ItemType type = ItemType.Default)
    {
        var filtered = items.FindAll(x => x != null && x.type == type);

        return filtered;
    }

    public ItemObject Get(int index) { return items[index]; }
}
