using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    StateMachine machine;
    StateMachine inventory;

    CharacterController controller;

    void Awake()
    {
        #region Get Component References

        controller = GetComponent<CharacterController>();
       // visualDetector = GetComponent<VisualDetector>();

        #endregion

        machine = new StateMachine();
        inventory = new StateMachine();

        Func<bool> Moving = () => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        Func<bool> Idle = () => Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") == 0;

        Func<bool> Aiming = () => Input.GetMouseButton(1);

        /*Func<>*/

        IdleState idle = new IdleState();
        GroundedState grounded = new GroundedState(controller);

        StowedState stowed = new StowedState(controller);
        AimState aimed = new AimState(controller);

        machine.At(idle, grounded, Moving);
        machine.At(grounded, idle, Idle);

        inventory.At(stowed, aimed, Aiming);

        // set default state
        machine.SetState(idle);
        //inventory.SetState(stowed);
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
