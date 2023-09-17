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
    
    List<Collider> targets = new List<Collider>();

    Collider activeTarget;

    PriorityType priority = PriorityType.Other;

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
            eyes.RegisterListener(OnTargetEntered, OnTargetExited);
        }

        if (ears != null )
        {
            ears.RegisterListener(OnTargetEntered);
        }
        #endregion

        machine = new StateMachine();

        // transition predicates
        Func<bool> BeginWander = () => Input.GetKeyDown(KeyCode.Space);
        Func<bool> BeginInvestigateVisual = () => priority == PriorityType.Visual;
        Func<bool> BeginInvestigateAudio = () => priority == PriorityType.Audio;
        Func<bool> SpottedPriorityVisual = () => priority == PriorityType.Visual && activeTarget != null;
        Func<bool> StopInvestigation = () => Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 10f;
        Func<bool> StartChase = () => activeTarget != null;

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

    public void ResetPriority()
    {

    }

    public void OnTargetEntered(Collider other)
    {
        if (!targets.Contains(other)) targets.Add(other);
        Debug.Log("Soemthing with eneitty/?");
        
    }

    public void OnTargetExited(Collider other)
    {
        if (targets.Contains(other)) targets.Remove(other);
    }

    void Update()
    {
        machine.Update();
    }
}