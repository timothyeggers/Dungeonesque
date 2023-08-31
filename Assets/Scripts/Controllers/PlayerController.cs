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
        // visualDetector = GetComponent<VisualDetector>();
        weaponSelector = GetComponent<WeaponSelector>();
        #endregion

        machine = new StateMachine();

        Func<bool> Moving = () => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        Func<bool> Idle = () => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0;

        Func<bool> Aiming = () => Input.GetMouseButton(1);

        /*Func<>*/

        IdleState idle = new IdleState();
        GroundedState grounded = new GroundedState(controller);

        StowedState stowed = new StowedState(controller);
        AimState aimed = new AimState(controller);

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

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            Debug.DrawLine(transform.position, hit.point, Color.white);
        }
    }
}
