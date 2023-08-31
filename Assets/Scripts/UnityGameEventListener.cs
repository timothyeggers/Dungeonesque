using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UnityGameEventListener : MonoBehaviour
{
    [SerializeField]
    GameEventSO _event;

    [SerializeField]
    UnityEvent<GameObject> onEvent_GameObject;

    [SerializeField]
    UnityEvent<Component, GameObject> onEvent_Component_GameObject;

    [SerializeField]
    UnityEvent<Component, GameObject, object> onEvent;   


    private void OnEnable()
    {
        _event.RegisterListener(this);
    }

    public void Raise(GameObject go)
    {
        onEvent_GameObject.Invoke(go);
    }

    public void Raise(Component sender, GameObject go)
    {
        onEvent_Component_GameObject.Invoke(sender, go);
    }


    public void Raise(Component sender, GameObject go, object data) 
    {
        onEvent.Invoke(sender, go, data);
    }

}
