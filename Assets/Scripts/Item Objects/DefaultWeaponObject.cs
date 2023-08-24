
using UnityEngine;


[CreateAssetMenu(fileName = "Fists", menuName = "Inventory/Fists")]
public class DefaultWeaponObject : ItemObject, IWeapon
{
    [SerializeField]
    private float _damage = 5f;

    [SerializeField]
    private float _fireRate = 0.75f;

    [SerializeField]
    private int _ammo = 5;

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

    public float stowTime { get => _stowTime; set => _stowTime = value; }

    public float unstowTime { get => _unstowTime; set => _unstowTime = value; }

    public void Awake()
    {
        type = ItemType.Weapon;
    }
}
