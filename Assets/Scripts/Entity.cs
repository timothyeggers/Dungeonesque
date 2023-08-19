using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Entity : MonoBehaviour
{
    [SerializeField]

    https://www.youtube.com/watch?v=7_dyDmF0Ktw
    List<ITarget> targets = new List<ITarget>();

    NavMeshAgent agent;
    StateMachine machine;
    TargetGenerator targetGenerator;

    void Awake()
    {
        #region Get Component References
        
        agent = GetComponent<NavMeshAgent>();
        targetGenerator = GetComponent<TargetGenerator>();

        #endregion

        machine = new StateMachine();

        // transition predicates
        Func<bool> BeginWander = () => Input.GetKeyDown(KeyCode.A);
        Func<bool> BeginInvestigateVisual = () => Input.GetMouseButtonDown(0);
        Func<bool> BeginInvestigateAudio = () => Input.GetMouseButtonDown(1);

        // initialize states
        IdleState idle = new IdleState();
        WanderState wander = new WanderState(agent, new RangeFloat(5, 5), 5f, 50f);
        InvestigateState investigate = new InvestigateState(agent, new RangeFloat(5, 5), 5f, 50f);

        // add transitions
        void At(IState from, IState to, Func<bool> predicate) => machine.AddTransition(from, to, predicate);

        At(idle, wander, BeginWander);
        At(wander, investigate, BeginInvestigateVisual);
        At(wander, investigate, BeginInvestigateAudio);

        // set default state
        machine.SetState(idle);
    }

    void Update()
    {
        machine.Update();
    }
}