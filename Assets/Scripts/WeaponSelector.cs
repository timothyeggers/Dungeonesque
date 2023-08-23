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
            Equip(1);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            Equip(0);
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
            var equipable = inventory.Get(ItemType.Equipable, equipped);
            if (equipable.GetType() == typeof(IWeapon))
            {
                var weapon = (IWeapon)equipable;
                yield return new WaitForSeconds(weapon.stowTime);
                equipped = 0;

                Debug.Log($"UnEquipped: {weapon}");
            }
        }

        if (queueEquip is int queue)
        {
            var equipable = inventory.Get(ItemType.Equipable, queue);
            if (equipable is IWeapon)
            {
                var weapon = (IWeapon)equipable;
                yield return new WaitForSeconds(weapon.unstowTime);
                equipped = queue;

                Debug.Log($"Equipped: {weapon}");
            }
        }
    }
}

