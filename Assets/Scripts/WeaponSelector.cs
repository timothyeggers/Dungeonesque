using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;


public class WeaponSelector : MonoBehaviour
{
    public GameObject Equipped { get; private set; }

    [SerializeField]
    InventoryObject inventory;

    List<GameObject> cachedInScene = new List<GameObject>();

    int equipped = 0;
    int? queueEquip = null;

    private void Awake()
    {
        
    }

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

    public void Equip(int index)
    {
        if (queueEquip != null) return;
        if (index == equipped) return;

        queueEquip = index;
        StartCoroutine(EquipRoutine());
    }

    IEnumerator EquipRoutine()
    {
        if (equipped > 0)
        {
            var equipable = inventory.GetWeaponOrNext(equipped);
            yield return new WaitForSeconds(equipable.stowTime);

            equipped = 0;

            if (Equipped) Equipped.SetActive(false);
            Equipped = null;
        }

        if (queueEquip is int queue)
        {
            var equipable = inventory.GetWeaponOrNext(queue);
            yield return new WaitForSeconds(equipable.unstowTime);
            
            equipped = queue;
            queueEquip = null;
            Debug.Log($"Equipped: {equipable}");

            if (Equipped) Equipped.SetActive(false);
            
            if (equipable.prefab == null) yield break;

            if (equipable.prefabInScene)
            {
                var prefab = cachedInScene.Find(x => x.scene.IsValid());
                // need to write better check if its already in scene or something
                Equipped = prefab;
                Equipped.SetActive(true);
            }
            else
            {
                Equipped = equipable.prefab;
                Equipped.SetActive(true);
                cachedInScene.Add(Instantiate(Equipped));
                equipable.prefabInScene = true;
            }
        }
    }
}

