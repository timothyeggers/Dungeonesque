using UnityEngine;


[CreateAssetMenu(fileName = "Bottle", menuName = "Inventory/Bottle")]
public class BottleObject : ItemObject, IWeapon
{
    public float damage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float fireRate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int ammo { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int capacity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Attack(GameObject sender)
    {
        Debug.Log($"Should attack. Called from: {sender}");
    }
}