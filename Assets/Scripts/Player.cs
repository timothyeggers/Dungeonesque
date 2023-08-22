using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    StateMachine machine;

    CharacterController controller;

    void At(IState from, IState to, Func<bool> predicate) => machine.AddTransition(from, to, predicate);

    void Awake()
    {
        #region Get Component References

        controller = GetComponent<CharacterController>();
       // visualDetector = GetComponent<VisualDetector>();

        #endregion

        machine = new StateMachine();

        Func<bool> Moving = () => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        Func<bool> Idle = () => Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") == 0;

        /*Func<>*/

        IdleState idle = new IdleState();
        GroundedState grounded = new GroundedState(controller);

        At(idle, grounded, Moving);
        At(grounded, idle, Idle);

        // set default state
        machine.SetState(idle);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        machine.Update();


    }
}
