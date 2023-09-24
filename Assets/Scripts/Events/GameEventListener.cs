using UnityEngine;
using UnityEngine.Events;

namespace Dungeonesque.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEventSO _event;

        [SerializeField] private UnityEvent<GameObject> onEvent_GameObject;

        [SerializeField] private UnityEvent<Component> onEvent_Component;

        [SerializeField] private UnityEvent<Component, GameObject> onEvent_Component_GameObject;

        [SerializeField] private UnityEvent<Component, GameObject, object> onEvent;


        private void OnEnable()
        {
            _event.RegisterListener(this);
        }

        public void Raise(GameObject go)
        {
            onEvent_GameObject.Invoke(go);
        }

        public void Raise(Component sender)
        {
            onEvent_Component.Invoke(sender);
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
}