using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
using Object = System.Object;

public class PlayerGroundedState : IState
{
    CharacterController controller;
    float speed;
    float runMod;
    float aimMod;

    public PlayerGroundedState(CharacterController controller, float speed = 6f, float runMod = 1.5f, float aimMod = 0.75f) { 
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

        controller?.Move( velocity * Time.deltaTime );
        controller?.Move(Time.deltaTime * Physics.gravity);
        controller.transform.LookAt(controller.transform.position + velocity.normalized);
    }
}

public class PlayerAttackState : IState
{
    CharacterController controller;
    MeleeController melee;
    VisualDetector targetSystem;
    float speed;

    List<Collider> targets = new List<Collider>();

    Collider activeTarget;

    public PlayerAttackState(CharacterController controller, MeleeController melee, VisualDetector targetSystem, float speed = 3f)
    {
        this.controller = controller;
        this.speed = speed;
        this.targetSystem = targetSystem;
    }

    public void OnEnter()
    {
        targetSystem.RegisterListener(OnTargetEntered, OnTargetExited);
        Debug.Log("Attacking.");
    }

    public void OnExit()
    {
        targetSystem.RemoveListener(OnTargetEntered, OnTargetExited);
        Debug.Log("Not attacking.");
    }

    public void Update()
    {
        // determine best activeTarget
        // closest activeTarget, Vector3.Distance is expensive so we should do something else about this later
        if (activeTarget && (activeTarget.gameObject.activeSelf == false || !targets.Contains(activeTarget))) {
            activeTarget = null;
        }

        if (activeTarget == null)
        {
            float distanceToCurrent = Mathf.Infinity;
            foreach (var target in targets)
            {
                var distance = Vector3.Distance(controller.transform.position, target.transform.position);
                if (distance < distanceToCurrent)
                {
                    activeTarget = target;
                    distanceToCurrent = distance;
                }
            }
        }


        var input_direction = Vector3.zero;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        input_direction.x = horizontal;
        input_direction.z = vertical;

        var velocity = input_direction * speed;

        controller?.Move(velocity * Time.deltaTime);
        controller?.Move(Time.deltaTime * Physics.gravity);

        var lookAt = velocity.normalized;
        if (activeTarget) lookAt = activeTarget.transform.position;
        lookAt.y = 0f;

        controller.transform.LookAt(lookAt);
    }

    public void OnTargetEntered(Collider other)
    {
        if (!targets.Contains(other)) targets.Add(other);
    }

    public void OnTargetExited(Collider other)
    {
        if (targets.Contains(other)) targets.Remove(other);
    }
}

