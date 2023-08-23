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

    public Dictionary<ItemType, List<ItemObject>> items = new Dictionary<ItemType, List<ItemObject>>();

/*    [NonSerialized]
    WeaponSO empty;

    private void OnEnable()
    {
        empty = CreateInstance<WeaponSO>();
    }
*/
    public void Add(ItemObject item)
    {
        if (items.ContainsKey(item.type)) {
            items[item.type].Add(item);
        } else
        {
            items[item.type] = new List<ItemObject>() { item };
        }
    }

    public ItemObject Get(ItemType type, int index)
    {
        if (items.ContainsKey(type))
        {
            var items = this.items[type];

            if (index > items.Count) return items[-1];
            if (index < 0) return items[0];

            return items[index];
        }
        return CreateInstance<DefaultWeapon>();
        
    }
}
