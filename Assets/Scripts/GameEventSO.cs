using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Game Event")]
public class GameEventSO : ScriptableObject
{
    [NonSerialized]
    private List<UnityGameEventListener> gameEventListeners = new List<UnityGameEventListener>();

    public void RegisterListener(UnityGameEventListener listener)
    {
        gameEventListeners.Add(listener);
    }

    public void Raise(GameObject go)
    {
        gameEventListeners.ForEach(x => x.Raise(go));
    }

    public void Raise(Component sender, GameObject go)
    {
        gameEventListeners.ForEach(x => x.Raise(sender, go));
    }

    public void Raise(Component sender, GameObject go, object data)
    {
        gameEventListeners.ForEach(x => x.Raise(sender, go, data));
    }
}