using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MeleeController melee;
    [SerializeField] private VisualDetector visualDetector;

    StateMachine machine;

    CharacterController controller;
    WeaponSelectorController weaponSelector;

    private List<Collider> eyesColliders = new List<Collider>();

    void Awake()
    {

        #region Get Component References
        controller = GetComponent<CharacterController>();
        weaponSelector = GetComponent<WeaponSelectorController>();
        #endregion

        #region State Machine Setup
        machine = new StateMachine();

        PlayerIdleState idle = new PlayerIdleState(controller);
        PlayerGroundedState grounded = new PlayerGroundedState(controller);
        PlayerAttackState attacking = new PlayerAttackState(controller, melee, visualDetector);

        
        #endregion

        #region State Machine Transitions
        Func<bool> Moving = () => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        Func<bool> Idle = () => Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0;
        Func<bool> Attacking = () => melee.IsPreparingAttack();
        Func<bool> DoneAttacking = () => !melee.IsAttacking();
        
        machine.At(idle, grounded, Moving);
        machine.At(grounded, idle, Idle);
        machine.Any(attacking, Attacking);
        machine.At(attacking, grounded, DoneAttacking);
        machine.At(attacking, idle, DoneAttacking);
        #endregion

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
            /*Debug.DrawLine(transform.position, hit.point, Color.white);*/
        }
    }
}
