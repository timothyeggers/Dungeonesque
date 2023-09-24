using Extensions;
using UnityEngine;
using Dungeonesque.Core;

namespace Dungeonesque.StateMachine.States
{
    public class PlayerGroundedState : IState
    {
        private readonly float aimMod;
        private readonly CharacterController controller;
        private readonly float rotationSpeed;
        private readonly float runMod;
        private readonly float speed;

        public PlayerGroundedState(CharacterController controller, float rotationSpeed = 15f, float speed = 6f,
            float runMod = 1.5f, float aimMod = 0.75f)
        {
            this.controller = controller;
            this.speed = speed;
            this.runMod = runMod;
            this.aimMod = aimMod;
            this.rotationSpeed = rotationSpeed;
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
            var inputDirection = Vector3.zero;

            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var running = Input.GetKey(KeyCode.LeftShift);

            var aiming = Input.GetMouseButton(0) && !running;

            inputDirection.x = horizontal;
            inputDirection.z = vertical;

            var velocity = inputDirection * speed;
            velocity *= running ? runMod : 1;
            velocity *= aiming ? aimMod : 1;


            controller.Move(velocity * Time.deltaTime);
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