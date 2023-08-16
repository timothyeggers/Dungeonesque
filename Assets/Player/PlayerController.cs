using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float runSpeedRatio = 1.5f;

    [SerializeField]
    private float rotationSpeed = 5.0f;

    [SerializeField]
    private LayerMask interactMask;

    [SerializeField]
    private LayerMask moveableMask = 1;


    private bool running = false;
    private NavMeshAgent agent;
    private Vector3? destination;

    private float[] angles = new float[] { 80f, 45f, 20f };
    private float speed = 0f;

    private bool canInteract = true;

    // Start is called before the first frame update                            
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        speed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3);

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (canInteract && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, interactMask))
            {
                Debug.Log("Clicked on: " + hit.collider.gameObject);
                StartCoroutine(interactCooldown());


                var obj = hit.collider.gameObject;
                if (obj.TryGetComponent<EnemyController>(out var controller))
                {
                    controller.Hit(this.gameObject);
                }
            }
            else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, moveableMask))
            {
                agent.SetDestination(hit.point);
            }
            
        }

        if (Input.GetMouseButton(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity)) {
                var dir = hit.point - transform.position;
                dir.y = 0f;
                transform.forward = Vector3.RotateTowards(transform.forward, dir.normalized, Time.deltaTime * rotationSpeed, 0f);
            }
        }

        running = (Input.GetKey(KeyCode.LeftShift));

        var direction = destinationDirection();
        if (direction != Vector3.zero)
        {
            var factor = speedFactor(direction) * (running ? runSpeedRatio : 1f);
            agent.speed = speed * factor;
            animator.SetFloat("Speed", factor);
        } else
        {
            animator.SetFloat("Speed", 0f);
        }
    }
    
    Vector3 destinationDirection()
    {
        var direction = agent.destination - transform.position;
        direction.y = 0f;
        return direction.normalized;
    }

    float speedFactor(Vector3 direction)
    {
        var forward = new Vector3(transform.forward.x, 0f, transform.forward.z);
        var angle = Vector3.Angle(direction, forward);

        if (angle >= angles[0])
        {
            return 0.65f;
        }
        if (angle >= angles[1])
        {
            return 0.8f;
        }

        return 1.0f;
    }

    IEnumerator interactCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.15f);
        canInteract = true;
    }
}
