using System;
using System.Collections.Generic;
using Dungeonesque.StateMachine;
using Dungeonesque.StateMachine.States;
using Dungeonesque.Triggers;
using Dungeonesque.Weapons;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private MeleeController melee;
    [SerializeField] private VisualTrigger visualDetector;

    private CharacterController controller;

    private List<Collider> eyesColliders = new();

    private StateMachine machine;
    private ItemSelector weaponSelector;

    private void Awake()
    {
        #region Get Component References

        controller = GetComponent<CharacterController>();
        weaponSelector = GetComponent<ItemSelector>();

        #endregion

        #region State Machine Setup

        machine = new StateMachine();

        var idle = new PlayerIdleState(controller);
        var grounded = new PlayerGroundedState(controller);
        var attacking = new PlayerAttackState(controller, visualDetector);

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
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
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