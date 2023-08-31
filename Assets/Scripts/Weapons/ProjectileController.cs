using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    IWeapon weaponData;

    bool monitoring = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (monitoring == false) return;
        var force_on_hit = -collision.impulse;
    }
}