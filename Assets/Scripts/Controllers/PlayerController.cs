using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    StateMachine machine;

    CharacterController controller;
    WeaponSelector weaponSelector;

    void Awake()
    {
        #region Get Component References
        controller = GetComponent<CharacterController>();
        weaponSelector = GetComponent<WeaponSelector>();
        #endregion

        machine = new StateMachine();

        Func<bool> Moving = () => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        Func<bool> Idle = () => Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0;
        
        PlayerIdleState idle = new PlayerIdleState(controller);
        PlayerGroundedState grounded = new PlayerGroundedState(controller);

        machine.At(idle, grounded, Moving);
        machine.At(grounded, idle, Idle);

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

        // draw debug line to mouse, with depth.
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            Debug.DrawLine(transform.position, hit.point, Color.white);
        }
    }

}
