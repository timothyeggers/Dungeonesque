using System.Collections.Generic;
using Dungeonesque.Inventory.Items;
using UnityEngine;

namespace Dungeonesque.Inventory
{
    [CreateAssetMenu(fileName = "Inventory Object")]
    public class InventoryObject : ScriptableObject
    {
        [SerializeField] private List<ItemObject> items = new();

        public void Add(ItemObject item)
        {
            items.Add(item);
        }

        public List<ItemObject> GetItems(ItemType type = ItemType.Default)
        {
            var filtered = items.FindAll(x => x != null && x.type == type);

            return filtered;
        }

        public ItemObject Get(int index)
        {
            return items[index];
        }
    }
}