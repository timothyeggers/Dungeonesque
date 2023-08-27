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
    public GameObject EquippedPrefab { get; private set; }     

    [SerializeField]
    InventoryObject inventory;

    [SerializeField]
    private ItemObject emptyWeapon;

    private List<GameObject> cachedPrefabs = new List<GameObject>();

    
    int equipped = 0;
    int? queueEquip = null;


    private void Update()
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
        if (queueEquip != null) return;
        if (index == equipped) return;

        queueEquip = index;
        StartCoroutine(EquipRoutine());
    }

    IEnumerator EquipRoutine()
    {
        var weapon = GetWeaponOrNext(equipped);
        yield return new WaitForSeconds(weapon.stowTime);

        equipped = 0;
        if (EquippedPrefab) EquippedPrefab.SetActive(false);
        EquippedPrefab = null;
        
        if (queueEquip is int queue)
        {
            weapon = GetWeaponOrNext(queue);
            yield return new WaitForSeconds(weapon.unstowTime);
            
            equipped = queue;
            queueEquip = null;
            Debug.Log($"Equipped: {weapon}, {equipped}");
            
            if (weapon.prefab == null) yield break;

            var prefab = cachedPrefabs.Find(x => x.scene.IsValid() && x.GetInstanceID() == weapon.prefabInstanceId);
            
            if (prefab != null)
            {
                EquippedPrefab = prefab;
                EquippedPrefab.SetActive(true);
            } else
            {
                EquippedPrefab = Instantiate(weapon.prefab);
                EquippedPrefab.SetActive(true);
                cachedPrefabs.Add(EquippedPrefab);
                weapon.prefabInstanceId = EquippedPrefab.GetInstanceID();
            }
        }
    }
}

