using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class InvestigateState : IState
{
    public delegate void InvestigationFinished();    

    // init
    NavMeshAgent agent;
    float timeToWait;
    InvestigationFinished onFinished;

    // Agent investigation
    Vector3 startPos;
    Vector3? queueDestination;

    MonoBehaviour coroutine;

    public InvestigateState(NavMeshAgent agent, float timeToWait, InvestigationFinished onFinished)
    {
        this.agent = agent;
        this.timeToWait = timeToWait;
        this.startPos = agent.transform.position;
        this.onFinished = onFinished;
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
        if (queueDestination is Vector3 queue)
        {
            agent.SetDestination(queue);
            queueDestination = null;
        }

        if (agent.remainingDistance < 1f)
        {
            coroutine.StartCoroutine(ScoutAreaRoutine());
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        queueDestination = position;
    }

    private IEnumerator ScoutAreaRoutine()
    {
        yield return new WaitForSeconds(timeToWait);
        onFinished();
    }
}