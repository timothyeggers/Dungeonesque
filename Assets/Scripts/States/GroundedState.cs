using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class GroundedState : IState
{
    CharacterController controller;
    float speed;
    float runMod;
    float aimMod;

    public GroundedState(CharacterController controller, float speed = 6f, float runMod = 1.5f, float aimMod = 0.7f) { 
        this.controller = controller;
        this.speed = speed;
        this.runMod = runMod;
        this.aimMod = aimMod;
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

        var aiming = Input.GetMouseButtonDown(0) && !running;

        input_direction.x = horizontal;
        input_direction.z = vertical;

        var velocity = input_direction * speed;
        velocity *= (running ? runMod : 1);
        velocity *= (aiming ? aimMod : 1);

        controller?.Move( velocity * Time.deltaTime);
        controller?.Move(Time.deltaTime * Physics.gravity);
    }
}

