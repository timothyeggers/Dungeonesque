using System;
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
    private Interactable[] interactables;

    private NavMeshAgent agent;
    private bool running = false;
    private Vector3? destination;

    private float[] angles = new float[] { 80f, 45f, 20f };
    private float speed = 0f;


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

        var interacted = Input.GetMouseButtonDown(0);
        var interact = Input.GetMouseButton(0);

        if (interact) {
            RaycastHit hit;

            foreach (var interactable in this.interactables)
            {
                print(interactable.Interact(out hit));

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

    public void Move(NavMeshAgent agent)
    {
        throw new NotImplementedException();
    }
}

// problem - we want to click on an interactable, have it walk to it, and interact after reaching.
// we want to click on an enemy, and attack an enemy if its in range
// both of these require a range of interaction, and a cooldown (aka how often you can interact)
// we could build a struct Interact, which accepts a physics layer mask, and cooldown, and range, and also accept a callback function

public delegate bool Interact(int hwnd, int lParam);

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Interactable", order = 1)]
class Interactable : ScriptableObject
{
    public LayerMask layerMask;
    public float coolDown;
    public float range;

    private bool canInteract;

    public bool CanInteract { get { return canInteract; } } 

    public IEnumerator CooldownRoutine()
    {
        canInteract = false;
        yield return new WaitForSeconds(coolDown);
        canInteract = true;
    }

    public virtual bool Interact(out RaycastHit hit)
    {
        var interacted = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask);
        return interacted;
    }
}

class Moveable : Interactable
{
    public override bool Interact(out RaycastHit hit)
    {
        var interacted = base.Interact(out hit);
        if (interacted)
        {

        }
        return interacted;
    }
}
