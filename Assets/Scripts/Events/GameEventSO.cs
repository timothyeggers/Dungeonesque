using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonesque.Events
{
    [CreateAssetMenu(fileName = "Game Event")]
    public class GameEventSO : ScriptableObject
    {
        [NonSerialized] private readonly List<GameEventListener> gameEventListeners = new();

        public void RegisterListener(GameEventListener listener)
        {
            gameEventListeners.Add(listener);
        }

        public void Raise(GameObject go)
        {
            gameEventListeners.ForEach(x => x.Raise(go));
        }

        public void Raise(Component sender)
        {
            gameEventListeners.ForEach(x => x.Raise(sender));
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
}