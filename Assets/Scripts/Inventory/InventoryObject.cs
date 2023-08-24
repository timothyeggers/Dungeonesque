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
    /*    [SerializeField]
        List<WeaponSO> weapons = new List<WeaponSO>();
    */
    [SerializeField]
    private List<ItemObject> items = new List<ItemObject>();
    
    /*public Dictionary<ItemType, List<ItemObject>> items = new Dictionary<ItemType, List<ItemObject>>();*/

/*    [NonSerialized]
    WeaponSO empty;

    private void OnEnable()
    {
        empty = CreateInstance<WeaponSO>();
    }
*/
    public void Add(ItemObject item)
    {
        items.Add(item);
    }

    public List<ItemObject> GetWeapons()
    {
        var filtered = items.FindAll(x => x != null && x.type == ItemType.Weapon);
        filtered.Insert(0, CreateInstance<DefaultWeaponObject>());

        return filtered;
    }

    public ItemObject GetWeaponOrNext(int index)
    {
        var filtered = GetWeapons();
        
        if (index > filtered.Count - 1)
        {
            var remainder = index % filtered.Count;
            return filtered[remainder];
        }

        return filtered[index]; 
    }

    public ItemObject Get(int index) { return items[index]; }
}
