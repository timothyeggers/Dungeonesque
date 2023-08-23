using UnityEngine;

public interface IWeapon
{
    public float damage { get; set; }

    public float fireRate { get; set; }

    public int ammo { get; set; }

    public int capacity { get; set; }

    public float stowTime { get; set; }

    public float unstowTime { get; set; }
}


