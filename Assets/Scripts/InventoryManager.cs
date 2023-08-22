using System.Collections;
using UnityEngine;


public class InventoryController : MonoBehaviour
{
    [SerializeField]
    Inventory inventory;

    int equipped = 0;
    int? queueEquip;

    void Update()
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
            yield return new WaitForSeconds(inventory.Get(equipped).stowTime);
            equipped = 0;
            Debug.Log($"UnEquipped: {inventory.Get(equipped)}");
        }
        
        if (queueEquip is int queue)
        {
            yield return new WaitForSeconds(inventory.Get(queue).unstowTime);
            equipped = queue;
            queueEquip = null;
            Debug.Log($"Equipped: {inventory.Get(equipped)}");
        }
    }
}
