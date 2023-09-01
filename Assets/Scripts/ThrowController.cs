using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    [SerializeField]
    float throwStrength = 9f;

    [SerializeField]
    LayerMask throwableLayer;

    public ItemObject item;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, throwableLayer))
            {
                Throw(transform.position, hit.point);
            }

            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
    }

    public void Throw(Vector3 from, Vector3 to, bool shouldArc = false)
    {
        var item = Instantiate(this.item.prefab);
        item.transform.position = from;
        var direction = (to - from).normalized;
        Debug.Log(direction);
        if (item.TryGetComponent<Rigidbody>(out var body))
        {
            body.excludeLayers = 1 << gameObject.layer;
            body.AddForce(direction * (throwStrength - body.mass), ForceMode.Impulse);
        }
    }
}
