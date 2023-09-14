using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Progress;


public class WeaponSelectorController : MonoBehaviour
{
    public ItemObject Equipped { get; private set; }

    [SerializeField]
    InventoryObject inventory;

    [SerializeField]
    private ItemObject emptyWeapon;
            
    int current;
    int? queue;

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Equip(current + 1);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            Equip(current - 1);
        }

    }
    
    public ItemObject GetWeaponOrNext(int index)
    {
        var filtered = inventory.GetItems();
        filtered.Insert(0, emptyWeapon);

        if (index > filtered.Count - 1)
        {
            var remainder = index % filtered.Count;
            return filtered[remainder];
        }

        return filtered[index];
    }

    public void Equip(int index)
    {
        if (queue != null) return;
        if (index == current) return;

        queue = index;
        StartCoroutine(EquipRoutine());
    }

    IEnumerator EquipRoutine()
    {
        var weapon = GetWeaponOrNext(current);
        yield return new WaitForSeconds(weapon.stowTime);

        Debug.Log($"Unequipped: {weapon}.");
        current = 0;
        Equipped = emptyWeapon;
        
        if (queue is int next)
        {
            weapon = GetWeaponOrNext(next);
            yield return new WaitForSeconds(weapon.unstowTime);

            Debug.Log($"Equipped: {weapon}.");
            current = next;
            queue = null;

            Equipped = weapon;
        }
    }
}

