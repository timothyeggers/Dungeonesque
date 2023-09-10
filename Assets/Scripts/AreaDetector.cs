using System.Collections.Generic;
using UnityEngine;

public class AreaDetector : MonoBehaviour, ITrigger
{
    public List<OnTriggerEntered> onTriggerEntered { get; } = new List<OnTriggerEntered>();

    public List<OnTriggerExited> onTriggerExited { get; } = new List<OnTriggerExited>();

    public void RegisterListener(OnTriggerEntered onTriggerEntered, OnTriggerExited onTriggerExited)
    {
        this.onTriggerEntered.Add(onTriggerEntered);
        this.onTriggerExited.Add(onTriggerExited);
    }

    public void OnTriggerEnter(Collider other)
    {
        onTriggerEntered.ForEach( x => x.Invoke(other) );
    }
}