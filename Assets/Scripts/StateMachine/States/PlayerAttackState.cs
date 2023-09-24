using System.Collections.Generic;
using Dungeonesque.Triggers;
using Dungeonesque.Weapons;
using UnityEngine;

namespace Dungeonesque.StateMachine.States
{
    public class PlayerAttackState : IState
    {
        private readonly CharacterController _controller;
        private readonly float _speed;
        private readonly VisualTrigger _targetSystem;
        
        private List<Collider> _targets = new();
        private Collider _activeTarget;
        private MeleeController _melee;

        public PlayerAttackState(CharacterController controller, VisualTrigger targetSystem, float speed = 3f)
        {
            _controller = controller;
            _speed = speed;
            _targetSystem = targetSystem;
        }

        public void OnEnter()
        {
            _targetSystem.RegisterListener(OnTargetEntered, OnTargetExited);
        }

        public void OnExit()
        {
            _activeTarget = null;
        }

        public void Update()
        {
            // determine best activeTarget
            // closest activeTarget, Vector3.Distance is expensive so we should do something else about this later
            if (_activeTarget && (_activeTarget.gameObject.activeSelf == false || !_targets.Contains(_activeTarget)))
                _activeTarget = null;

            if (_activeTarget == null)
            {
                var distanceToCurrent = Mathf.Infinity;
                foreach (var target in _targets)
                {
                    var distance = Vector3.Distance(_controller.transform.position, target.transform.position);
                    if (distance < distanceToCurrent)
                    {
                        _activeTarget = target;
                        distanceToCurrent = distance;
                    }
                }
            }
            
            var input_direction = Vector3.zero;

            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            input_direction.x = horizontal;
            input_direction.z = vertical;

            var velocity = input_direction * _speed;

            _controller?.Move(velocity * Time.deltaTime);
            _controller?.Move(Time.deltaTime * Physics.gravity);

            if (_activeTarget)
            {
                Debug.Log("There is a target");
                var lookAt = _activeTarget.transform.position;
                lookAt.y = _controller.transform.position.y;
                _controller.transform.LookAt(lookAt);
            }
        }

        public void OnTargetEntered(Collider other)
        {
            if (!_targets.Contains(other)) _targets.Add(other);
        }

        public void OnTargetExited(Collider other)
        {
            if (_targets.Contains(other)) _targets.Remove(other);
        }
    }
}