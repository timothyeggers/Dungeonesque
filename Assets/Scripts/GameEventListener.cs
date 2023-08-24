using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> {}

/*public class WeaponEvent : UnityEvent<DefaultWeaponObject> {}*/


public interface IGameEventListener<T0, T1>
{ 
    public void OnEventRaised(T0 sender, T1 data);
}


public class GameEventListener : MonoBehaviour, IGameEventListener<Component, object>
{

    [Tooltip("Event to register with.")]
    public GameEvent gameEvent;

    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public CustomGameEvent response;

    private void OnEnable() {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable() {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data) {
        response.Invoke(sender, data);
    }
}
