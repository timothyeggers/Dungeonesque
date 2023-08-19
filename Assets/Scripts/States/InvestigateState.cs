using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class InvestigateState : IState
{
    NavMeshAgent agent;
    StateMachine machine;
    Vector3 startPos;
    Vector3? queueDestination;

    RangeFloat waitTime;
    float radius;
    float maxWanderRadius;
    float waitFor = 0f;

    LayerMask enemyLayer;

    // a manual, overriding, hey this guys in this range
    // we ned to set an aggressor, so we need to check component types of 'hey is this x or y'
    // e need a list of recent aggressors with priorities to choose what to attack??
    // so like noise detector hits, depending on range, its like 2 aggression
    // but iv visual detector hits, dependong on same range, its likr 4 aggression
    // but if a wild bear is in sight, its like a 10 aggression
    // and finally if you attack the enemy, its like max aggression
    // funny its osunds liek we can use a statemachine
    public InvestigateState(NavMeshAgent agent, RangeFloat waitTime, float radius, float maxDistanceFromStart = 0f)
    {
        

        this.agent = agent;
        this.waitTime = waitTime;
        this.radius = radius;
        this.maxWanderRadius = maxDistanceFromStart;
        this.startPos = agent.transform.position;

        this.enemyLayer = 1 << LayerMask.NameToLayer("Default");
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void Update()
    {
        if (waitFor <= 0f && queueDestination != null)
        {
            agent.SetDestination((Vector3) queueDestination);
            queueDestination = null;
        }
        else
        {
            waitFor -= Time.deltaTime;
        }

        if (waitFor <= 0f && DestinationReached()) // && queueDestination == null
        {
            waitFor = UnityEngine.Random.Range(waitTime.x, waitTime.y);
            // then enter wander state
            //queueDestination = GetNextPosition();
            // instead maybe we exit state if nothing is found
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        queueDestination = position;
    }

    private bool DestinationReached()
    {
        return (agent.remainingDistance < 1f);
    }
}