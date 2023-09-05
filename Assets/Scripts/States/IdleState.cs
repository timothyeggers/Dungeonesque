using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class IdleState : IState
{
    public IdleState() { }

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

