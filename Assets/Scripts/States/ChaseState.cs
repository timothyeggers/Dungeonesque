using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class ChaseState : IState
{
    public delegate void ChaseFinished();
    

    // init
    NavMeshAgent agent;
    GameObject target;
    ChaseFinished onFinished;
    float maxDistanceFromTarget;

    // Agent investigation
    Vector3 startPos;
    Vector3? queueDestination;

    MonoBehaviour coroutine;

    public ChaseState(NavMeshAgent agent, float maxDistanceFromTarget, ChaseFinished onFinished)
    {
        this.agent = agent;
        this.startPos = agent.transform.position;
        this.onFinished = onFinished;
        this.maxDistanceFromTarget = maxDistanceFromTarget;
/*        this.enemyLayer = 1 << LayerMask.NameToLayer("Default");*/
    }

    public void OnEnter()
    {
        coroutine = agent.gameObject.GetComponent<MonoBehaviour>();
    }

    public void OnExit()
    {
        coroutine = null;
    }

    public void Update()
    {
        agent.SetDestination(target.transform.position);

        if (agent.remainingDistance > maxDistanceFromTarget)
        {
            onFinished();
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}