using System.Collections.Generic;
using Dungeonesque.Core;
using UnityEngine;

namespace Dungeonesque.Triggers
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshCollider), typeof(MeshFilter))]
    public class VisualTrigger : MonoBehaviour, ITrigger
    {
        [Tooltip("The solid objects that will interrupt field of vision!")]
        [SerializeField] private LayerMask opaqueLayers;

        [SerializeField] private float fovWidth = 7f;
        [SerializeField] private float fovHeight = 5f;
        [SerializeField] private float fovDepth = 13f;

        private MeshCollider _meshCollider;

        public void OnEnable()
        {
            _meshCollider = GetComponent<MeshCollider>();
            _meshCollider.convex = true;
            _meshCollider.isTrigger = true;

            var filter = GetComponent<MeshFilter>();
            filter.sharedMesh = MeshBuilder.BuildPyramid(fovWidth, fovDepth - 1, fovHeight);
            filter.transform.rotation = Quaternion.Euler(-90, 0, 0);

            _meshCollider.sharedMesh = filter.sharedMesh;
        }

        public void OnTriggerEnter(Collider other)
        {
            var direction = other.transform.position - transform.position;
            direction = direction.normalized;

            var ray = new Ray(transform.position, direction);
            
            // OnTriggerEnter is builtin, if the mesh collider has something enter, make sure a raycast can reach said object
            // without hitting the opaque layers...
            if (Physics.Raycast(ray, out var hit, fovDepth, opaqueLayers) == false)
                onTriggerEntered.ForEach(x => x.Invoke(other));
        }

        public void OnTriggerExit(Collider other)
        {
            onTriggerExited.ForEach(x => x.Invoke(other));
        }

        public List<OnTriggerEntered> onTriggerEntered { get; } = new();
        public List<OnTriggerExited> onTriggerExited { get; } = new();

        public void RegisterListener(OnTriggerEntered onTriggerEntered, OnTriggerExited onTriggerExited)
        {
            if (!this.onTriggerEntered.Contains(onTriggerEntered)) this.onTriggerEntered.Add(onTriggerEntered);
            if (!this.onTriggerExited.Contains(onTriggerExited)) this.onTriggerExited.Add(onTriggerExited);
        }

        public void RemoveListener(OnTriggerEntered onTriggerEntered, OnTriggerExited onTriggerExited)
        {
            if (this.onTriggerEntered.Contains(onTriggerEntered)) this.onTriggerEntered.Remove(onTriggerEntered);
            if (this.onTriggerExited.Contains(onTriggerExited)) this.onTriggerExited.Remove(onTriggerExited);
        }
    }
}