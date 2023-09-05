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

    GameObject target;
    Priorities priority = Priorities.Other;

    #region States
    IdleState idle;
    WanderState wander;
    InvestigateState investigate;
    ChaseState chase;
    #endregion

    void At(IState from, IState to, Func<bool> predicate) => machine.AddTransition(from, to, predicate);

    void Any(IState to, Func<bool> predicate) => machine.AddAnyTransition(to, predicate);

    void Awake()
    {
        #region Get Component References
        agent = GetComponent<NavMeshAgent>();
        #endregion

        #region Register Eyes and Ears
        if (eyes != null )
        {
            eyes.RegisterListener(AddTarget, null);
        }

        if (ears != null )
        {
            ears.RegisterListener(AddTarget);
        }
        #endregion

        machine = new StateMachine();

        // transition predicates
        Func<bool> BeginWander = () => Input.GetKeyDown(KeyCode.Space);
        Func<bool> BeginInvestigateVisual = () => priority == Priorities.Visual;
        Func<bool> BeginInvestigateAudio = () => priority == Priorities.Audio;
        Func<bool> SpottedPriorityVisual = () => priority == Priorities.Visual && target != null;
        Func<bool> StopInvestigation = () => Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 10f;
        Func<bool> StartChase = () => target != null;

        // initialize states
        idle = new IdleState();
        wander = new WanderState(agent, new RangeFloat(0.25f, 5), 5f, 20f);
        investigate = new InvestigateState(agent, 5f, ResetPriority);
        chase = new ChaseState(agent, 5f, ResetPriority);


        // add static transitions
        At(idle, wander, BeginWander);
        At(idle, investigate, BeginInvestigateVisual);
        At(wander, investigate, BeginInvestigateVisual);
        At(idle, investigate, BeginInvestigateAudio);
        At(investigate, investigate, BeginInvestigateAudio);
        At(investigate, investigate, BeginInvestigateVisual);
        At(wander, investigate, BeginInvestigateAudio);
        At(investigate, wander, StopInvestigation);
        Any(chase, StartChase);

        // set default state
        machine.SetState(idle);
    }

    public void AddTarget(Collider sender)
    {
        Debug.Log($"Called from {sender}.");

        if (sender.gameObject.GetComponent<AudioTrigger>())
        {
            priority = Priorities.Audio;
            investigate.SetTargetPosition(sender.transform.position);
            Debug.Log("Entity will investigate audio notification.");
        }

        if (sender.gameObject.GetComponent<VisualNotifier>())
        {
            priority = Priorities.Visual;
            investigate.SetTargetPosition(sender.transform.position);
            Debug.Log("Entity will investigate visual notification.");
        }

    } 

    public void ResetPriority()
    {

    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        chase.SetTarget(target);
    }

    void Update()
    {
        machine.Update();
    }
}