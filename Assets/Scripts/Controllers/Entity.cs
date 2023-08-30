using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Entity : MonoBehaviour
{
    NavMeshAgent agent;
    VisualDetector visualDetector;
    StateMachine machine;
    Priorities priority = Priorities.Other;

    #region States
    IdleState idle;
    WanderState wander;
    InvestigateState investigate;
    #endregion

    void At(IState from, IState to, Func<bool> predicate) => machine.AddTransition(from, to, predicate);

    void Awake()
    {
        #region Get Component References
        
        agent = GetComponent<NavMeshAgent>();
        visualDetector = GetComponent<VisualDetector>();

        #endregion

        machine = new StateMachine();

        // transition predicates
        Func<bool> BeginWander = () => Input.GetKeyDown(KeyCode.A);
        Func<bool> BeginInvestigateVisual = () => priority == Priorities.Visual;
/*        Func<bool> BeginInvestigateAudio = () => priority == Priorities.Audio;*/
        Func<bool> StopInvestigation = () => Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 10f;

        // initialize states
        idle = new IdleState();
        wander = new WanderState(agent, new RangeFloat(0.25f, 5), 5f, 20f);
        investigate = new InvestigateState(agent);


        // add static transitions
        At(idle, wander, BeginWander);
        At(wander, investigate, BeginInvestigateVisual);
        At(investigate, wander, StopInvestigation);

        // set default state
        machine.SetState(wander);
    }

    public void SetPriority(Component sender, object data)
    {
        if (sender.GetType() == typeof(VisualNotifier)) {
            priority = Priorities.Visual;
            if (data is RaycastHit hit)
                investigate.SetTargetPosition(hit.point);
        }
    } 

    void Update()
    {
        machine.Update();
    }
}