using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class StowedState : IState
{
    CharacterController controller;
    float speed;
    float runMod;

    public StowedState(CharacterController controller, float speed = 6f, float runMod = 1.5f)
    {
        this.controller = controller;
        this.speed = speed;
        this.runMod = runMod;
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

        controller?.Move(input_direction * speed * (running ? runMod : 1) * Time.deltaTime);
        controller.Move(Time.deltaTime * Physics.gravity);
    }
}

