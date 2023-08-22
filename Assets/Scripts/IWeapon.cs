using UnityEngine;

public interface IWeapon
{
    public float damage { get; set; }

    public float fireRate { get; set; }

    public int ammo { get; set; }

    public int capacity { get; set; }
}


public class WeaponSO : ScriptableObject, IWeapon
{
    [SerializeField]
    private float _damage = 1f;

    [SerializeField]
    private float _fireRate = 0.75f;

    [SerializeField]
    private int _ammo = 8;

    [SerializeField]
    private int _capacity = -1;

    [SerializeField]
    private float _stowTime = 0.25f;

    [SerializeField]
    private float _unstowTime = 0.25f;

    public float damage { get => _damage; set => _damage = value; }

    public float fireRate { get => _fireRate; set => _fireRate = value; }

    public int ammo { get => _ammo; set => _ammo = value; }

    public int capacity { get => _capacity; set => _capacity = value; }

    public float stowTime { get => _stowTime; }

    public float unstowTime { get => _unstowTime; }
}


[CreateAssetMenu(fileName = "Bottle")]
public class Bottle : WeaponSO
{
    
}
