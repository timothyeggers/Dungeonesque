using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class PlayerGroundedState : IState
{
    CharacterController controller;
    float speed;
    float runMod;
    float aimMod;

    public PlayerGroundedState(CharacterController controller, float speed = 6f, float runMod = 1.5f, float aimMod = 0.7f) { 
        this.controller = controller;
        this.speed = speed;
        this.runMod = runMod;
        this.aimMod = aimMod;
    }

    public void OnEnter()
    {
        //throw new NotImplementedException();
    }

    public void OnExit()
    {
        //throw new NotImplementedException();
    }

    public void Update()
    {
        var input_direction = Vector3.zero;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var running = Input.GetKey(KeyCode.LeftShift);

        var aiming = Input.GetMouseButton(0) && !running;

        input_direction.x = horizontal;
        input_direction.z = vertical;

        var velocity = input_direction * speed;
        velocity *= (running ? runMod : 1);
        velocity *= (aiming ? aimMod : 1);

        controller?.Move( velocity * Time.deltaTime);
        controller?.Move(Time.deltaTime * Physics.gravity);
    }
}

