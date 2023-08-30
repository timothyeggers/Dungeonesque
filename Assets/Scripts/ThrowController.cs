using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    public ItemObject item;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                Throw(transform.position, hit.point);
            }
        }
    }

    public void Throw(Vector3 from, Vector3 to, bool shouldArc = false)
    {
        var item = Instantiate(this.item.prefab);
        item.transform.position = from;

        var direction = (to - from).normalized;
        var body = item.GetComponent<Rigidbody>();
        body.excludeLayers = 1 << gameObject.layer;
        body?.AddForce(direction * 15, ForceMode.Impulse);
    }
}
