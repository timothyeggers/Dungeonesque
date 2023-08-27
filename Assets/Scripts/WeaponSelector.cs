using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Progress;


public class WeaponSelector : MonoBehaviour
{
    public ItemObject Equipped { get; private set; }

    public GameObject EquippedPrefab { get; private set; }

    [SerializeField]
    InventoryObject inventory;

    [SerializeField]
    private ItemObject emptyWeapon;

    private List<GameObject> cachedPrefabs = new List<GameObject>();
        
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

    public void UseWeapon(GameObject sender)
    {
        if (Equipped is IWeapon weapon)
        {
            weapon.Attack(sender);
        }
    }
    
    public ItemObject GetWeaponOrNext(int index)
    {
        var filtered = inventory.GetWeapons();
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

        // Reset to empty hands
        current = 0;
        Equipped = emptyWeapon;

        if (EquippedPrefab) EquippedPrefab.SetActive(false);
        EquippedPrefab = null;
        
        // If there's a weapon in the equip queue
        if (queue is int next)
        {
            weapon = GetWeaponOrNext(next);
            yield return new WaitForSeconds(weapon.unstowTime);
                        
            current = next;
            queue = null;

            Equipped = weapon;

            if (weapon.prefab == null) yield break;

            CreateWeaponPrefab(weapon);
        }
    }

    private void CreateWeaponPrefab(ItemObject weapon)
    {
        var prefab = cachedPrefabs.Find(x => x.scene.IsValid() && x.GetInstanceID() == weapon.prefabInstanceId);

        if (prefab != null)
        {
            EquippedPrefab = prefab;
            EquippedPrefab.SetActive(true);
        }
        else
        {
            EquippedPrefab = Instantiate(weapon.prefab);
            EquippedPrefab.SetActive(true);
            cachedPrefabs.Add(EquippedPrefab);
            weapon.prefabInstanceId = EquippedPrefab.GetInstanceID();
        }
    }
}

