using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float sightRadius = 10f;

    [SerializeField]
    private bool chasingPlayer = false;

    private NavMeshAgent agent;
    private int playerMask;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        playerMask = 1 << LayerMask.NameToLayer("Player");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.forward * sightRadius);
        if (Physics.CheckSphere(transform.position, sightRadius, playerMask))
        {
            chasingPlayer = true;
        }

        if (chasingPlayer)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    public void Hit(GameObject from)
    {
        print("Is hit");
        var hitDir = transform.forward - from.transform.forward;
        transform.position -= hitDir * 2f;
    }
}
