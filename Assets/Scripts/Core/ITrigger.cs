using System.Collections.Generic;
using UnityEngine;

namespace Dungeonesque.Core
{
    public delegate void OnTriggerEntered(Collider other);

    public delegate void OnTriggerExited(Collider other);

    public interface ITrigger
    {
        public List<OnTriggerEntered> onTriggerEntered { get; }
        public List<OnTriggerExited> onTriggerExited { get; }

        public void RegisterListener(OnTriggerEntered onTriggerEntered, OnTriggerExited onTriggerExited);
    }
}