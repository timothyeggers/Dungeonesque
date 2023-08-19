using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class WanderState : IState
{
    NavMeshAgent agent;
    Vector3 startPos;
    Vector3? queueDestination;

    RangeFloat waitTime;
    float radius;
    float maxWanderRadius;
    float waitFor = 0f;


    public WanderState(NavMeshAgent agent, RangeFloat waitTime, float radius, float maxDistanceFromStart = 0f)
    {
        this.agent = agent;
        this.waitTime = waitTime;
        this.radius = radius;
        this.maxWanderRadius = maxDistanceFromStart;
        this.startPos = agent.transform.position;
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
            agent.SetDestination((Vector3)queueDestination);
            queueDestination = null;
        }
        else
        {
            waitFor -= Time.deltaTime;
        }

        if (DestinationReached() && queueDestination == null)
        {
            waitFor = UnityEngine.Random.Range(waitTime.x, waitTime.y);
            queueDestination = GetNextPosition();
        }
    }

    protected Vector3 GetNextPosition()
    {
        var point = UnityEngine.Random.onUnitSphere * radius;
        point.y = 0;
        point += agent.transform.position;

        if (Vector3.Distance(startPos, point) > maxWanderRadius)
        {
            return point.magnitude * 2 * (startPos - point).normalized;
        }
        return point;
    }

    private bool DestinationReached()
    {
        return (agent.remainingDistance < 1f);
    }
}