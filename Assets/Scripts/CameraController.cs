using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float maxPanDistance = 7f;

    [SerializeField]
    private LayerMask cameraPanMask = 1;
    
    private Camera camera;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
        offset = transform.position - target.position;
    }
     
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = offset + target.transform.position;
        
        if (Input.GetMouseButton(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, cameraPanMask))
            {
                var point = hit.point;
                var difference = target.transform.position - point;
                var direction = difference.normalized;
                var distance = Mathf.Min(maxPanDistance, difference.magnitude);
                var endPos = target.transform.position - direction * distance;

                transform.position = offset + endPos;
            }
        }
    }
}
