using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class InvestigateState : IState
{
    public delegate void DestinationReached();

    NavMeshAgent agent;
    Vector3 startPos;
    Vector3? queueDestination;

    LayerMask enemyLayer;
    DestinationReached callback;
        

    // a manual, overriding, hey this guys in this range
    // we ned to set an aggressor, so we need to check component types of 'hey is this x or y'
    // e need a list of recent aggressors with priorities to choose what to attack??
    // so like noise detector hits, depending on range, its like 2 aggression
    // but iv visual detector hits, dependong on same range, its likr 4 aggression
    // but if a wild bear is in sight, its like a 10 aggression
    // and finally if you attack the enemy, its like max aggression
    // funny its osunds liek we can use a statemachine
    public InvestigateState(NavMeshAgent agent, DestinationReached callback)
    {
        this.agent = agent;
        this.startPos = agent.transform.position;
        this.callback = callback;
        this.enemyLayer = 1 << LayerMask.NameToLayer("Default");
    }

    public void OnEnter()
    {
        Debug.Log("Should chase player.");
        agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    public void OnExit()
    {

    }

    public void Update()
    {
        if (queueDestination is Vector3 queue)
        {
            agent.SetDestination(queue);
            queueDestination = null;
        }

        if (agent.remainingDistance < 1f)
        {
            callback();
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        queueDestination = position;
    }
}