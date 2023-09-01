using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Entity : MonoBehaviour
{
    [SerializeField] VisualDetector eyes;
    [SerializeField] AudioDetector ears;

    NavMeshAgent agent;
    
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
        #endregion

        #region Register Eyes and Ears
        if (eyes != null )
        {
            eyes.RegisterListener(SetPriority);
        }

        if (ears != null )
        {
            ears.RegisterListener(SetPriority);
        }
        #endregion

        machine = new StateMachine();

        // transition predicates
        Func<bool> BeginWander = () => Input.GetKeyDown(KeyCode.Space);
        Func<bool> BeginInvestigateVisual = () => priority == Priorities.Visual;
        Func<bool> BeginInvestigateAudio = () => priority == Priorities.Audio;
        Func<bool> StopInvestigation = () => Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 10f;

        // initialize states
        idle = new IdleState();
        wander = new WanderState(agent, new RangeFloat(0.25f, 5), 5f, 20f);
        investigate = new InvestigateState(agent, ResetPriority);


        // add static transitions
        At(idle, wander, BeginWander);
        At(idle, investigate, BeginInvestigateVisual);
        At(wander, investigate, BeginInvestigateVisual);
        At(idle, investigate, BeginInvestigateAudio);
        At(investigate, investigate, BeginInvestigateAudio);
        At(investigate, investigate, BeginInvestigateVisual);
        At(wander, investigate, BeginInvestigateAudio);
        At(investigate, wander, StopInvestigation);

        // set default state
        machine.SetState(idle);
    }

    public void SetPriority(Component sender)
    {
        Debug.Log($"Called from {sender}.");
        if (sender is VisualNotifier)
        {
            priority = Priorities.Visual;
            investigate.SetTargetPosition(sender.transform.position);
            Debug.Log("Entity will investigate visual notification.");
        }

        if (sender is AudioTrigger)
        {
            priority = Priorities.Audio;
            investigate.SetTargetPosition(sender.transform.position);
            Debug.Log("Entity will investigate audio notification.");
        }
    } 

    public void ResetPriority()
    {
        priority = Priorities.Other;
    }

    void Update()
    {
        machine.Update();
    }
}