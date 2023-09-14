using UnityEngine;

public class PlayerIdleState : IdleState
{
    CharacterController controller;
    public PlayerIdleState(CharacterController controller) { 
        this.controller = controller;
    }

    public new void OnEnter()
    {
        //throw new NotImplementedException();
    }

    public new void OnExit()
    {
        base.OnExit();
        //throw new NotImplementedException();
    }

    public new void Update()
    {
        controller?.Move(Time.deltaTime * Physics.gravity);
    }
}
