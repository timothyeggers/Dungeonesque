using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon Stats", menuName = "Weapons/Default Weapon Stats")]
public class WeaponStatsObject : ScriptableObject, IWeapon
{
    public string description;

    [SerializeField]
    private float _damage = 5f;

    [SerializeField]
    private float _fireRate = 0.75f;

    [SerializeField]
    private int _ammo = 5;

    [SerializeField]
    private int _capacity = -1;

    public float damage { get => _damage; set => _damage = value; }

    public float fireRate { get => _fireRate; set => _fireRate = value; }

    public int ammo { get => _ammo; set => _ammo = value; }

    public int capacity { get => _capacity; set => _capacity = value; }
}