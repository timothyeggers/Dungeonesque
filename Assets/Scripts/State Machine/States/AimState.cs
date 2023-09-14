using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class AimState : IState
{
    CharacterController controller;
    float aimMod;

    public AimState(CharacterController controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        Debug.Log("Grounded?");
        //throw new NotImplementedException();
    }

    public void OnExit()
    {
        //throw new NotImplementedException();
    }

    public void Update()
    {
        var input_direction = Vector3.zero;

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var running = Input.GetKey(KeyCode.LeftShift);

        input_direction.x = horizontal;
        input_direction.z = vertical;
    }
}

