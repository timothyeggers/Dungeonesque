using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Default Item", menuName = "Inventory/Default")]
public class DefaultItemObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Default;
    }
}