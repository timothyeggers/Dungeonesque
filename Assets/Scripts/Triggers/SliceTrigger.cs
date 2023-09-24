using System.Collections.Generic;
using Dungeonesque.Core;
using EzySlice;
using UnityEngine;

namespace Dungeonesque.Triggers
{
    public class SliceTrigger : MonoBehaviour, ITrigger
    {
        [SerializeField] private Transform planeDirection;

        // swingPivot contact
        public void OnTriggerEnter(Collider other)
        {
            onTriggerEntered.ForEach(x => x.Invoke(other));
            SliceInstantiate(other.gameObject, other.ClosestPoint(transform.position), planeDirection.transform.up);
        }

        public List<OnTriggerEntered> onTriggerEntered { get; } = new();

        public List<OnTriggerExited> onTriggerExited { get; } = new();

        public void RegisterListener(OnTriggerEntered onTriggerEntered, OnTriggerExited onTriggerExited)
        {
            this.onTriggerEntered.Add(onTriggerEntered);
            this.onTriggerExited.Add(onTriggerExited);
        }

        public GameObject[] SliceInstantiate(GameObject objectToSlice, Vector3 planeWorldPosition,
            Vector3 planeWorldDirection)
        {
            var objs = objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection);
            objectToSlice.SetActive(false);
            foreach (var obj in objs)
            {
                var coll = obj.AddComponent<MeshCollider>();
                coll.convex = true;
                obj.AddComponent<Rigidbody>();
            }

            return objs;
        }
    }
}