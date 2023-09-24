using UnityEngine;

namespace Dungeonesque.StateMachine.States
{
    public class IdleState : IState
    {
        public void OnEnter()
        {
            //throw new NotImplementedException();
        }

        public void OnExit()
        {
            Debug.Log("Base OnExit Idle.");
            //throw new NotImplementedException();
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }
    }
}