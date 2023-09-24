using System.Collections;
using Dungeonesque.Inventory;
using Dungeonesque.Inventory.Items;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] private InventoryObject inventory;

    [SerializeField] private ItemObject empty;

    private int current;
    private int? queue;
    public ItemObject Equipped { get; private set; }

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0) Equip(current + 1);
        if (Input.mouseScrollDelta.y < 0) Equip(current - 1);
    }

    public ItemObject GetNextItem(int index)
    {
        var filtered = inventory.GetItems();
        filtered.Insert(0, empty);

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

    private IEnumerator EquipRoutine()
    {
        var item = GetNextItem(current);
        yield return new WaitForSeconds(item.stowTime);

        current = 0;
        Equipped = empty;

        if (queue is int next)
        {
            item = GetNextItem(next);
            yield return new WaitForSeconds(item.unstowTime);

            Debug.Log($"Equipped: {item}.");
            current = next;
            queue = null;

            Equipped = item;
        }
    }
}