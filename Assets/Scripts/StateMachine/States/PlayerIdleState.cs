using Extensions;
using UnityEngine;
using Dungeonesque.Core;

namespace Dungeonesque.StateMachine.States
{
    public class PlayerIdleState : IState
    {
        private readonly CharacterController controller;
        private readonly float rotationSpeed = 15f;

        public PlayerIdleState(CharacterController controller)
        {
            this.controller = controller;
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
            controller.Move(Time.deltaTime * Physics.gravity);

            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cameraRay, out var cameraRayHit, Mathf.Infinity, K.GROUND_LAYER))
            {
                var targetPosition = new Vector3(cameraRayHit.point.x, 0, cameraRayHit.point.z);
                controller.transform.LookAtSmooth(targetPosition, rotationSpeed);
            }
        }
    }
}