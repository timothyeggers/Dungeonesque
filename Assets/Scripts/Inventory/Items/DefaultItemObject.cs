using UnityEngine;

namespace Dungeonesque.Inventory.Items
{
    [CreateAssetMenu(fileName = "Default Item", menuName = "Inventory/Default Item")]
    public class DefaultItemObject : ItemObject
    {
        public void Awake()
        {
            type = ItemType.Default;
        }
    }
}