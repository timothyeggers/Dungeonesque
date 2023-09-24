using Dungeonesque.Inventory.Items;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    [SerializeField] private float throwStrength = 9f;
    [SerializeField] private LayerMask throwableLayer;

    public ItemObject Item;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, throwableLayer))
                Throw(transform.position, hit.point);

            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
    }

    private void Throw(Vector3 from, Vector3 to, bool shouldArc = false)
    {
        var item = Instantiate(Item.prefab);
        item.transform.position = from;
        var direction = (to - from).normalized;

        if (item.TryGetComponent<Rigidbody>(out var body))
        {
            body.excludeLayers = 1 << gameObject.layer;
            body.AddForce(direction * (throwStrength - body.mass), ForceMode.Impulse);
        }
    }
}