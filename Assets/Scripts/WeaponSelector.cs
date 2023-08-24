using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;


public class WeaponSelector : MonoBehaviour
{
    [SerializeField]
    InventoryObject inventory;

    int equipped = 0;
    int? queueEquip = null;

    public void Awake()
    {
        
    }

    public void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Equip(equipped + 1);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            Equip(equipped - 1);
        }

    }

    public void Equip(int index)
    {
        if (queueEquip != null) return;
        if (index == equipped) return;

        queueEquip = index;
        StartCoroutine(EquipRoutine());
    }

    public IEnumerator EquipRoutine()
    {
        if (equipped > 0)
        {
            var equipable = inventory.GetWeaponOrNext(equipped);
            if (equipable is IWeapon weapon)
            {
                yield return new WaitForSeconds(weapon.stowTime);
                equipped = 0;

                Debug.Log($"UnEquipped: {weapon}");
            }
        }

        if (queueEquip is int queue)
        {
            var equipable = inventory.GetWeaponOrNext(queue);
            if (equipable is IWeapon weapon)
            {
                yield return new WaitForSeconds(weapon.unstowTime);
                equipped = queue;
                queueEquip = null;

                Debug.Log($"Equipped: {weapon}");
            }
        }
    }
}

