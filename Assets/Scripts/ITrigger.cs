using System.Collections.Generic;


public delegate void OnTriggerEntered(UnityEngine.Collider other);
public delegate void OnTriggerExited(UnityEngine.Collider other);

public interface ITrigger
{
    public List<OnTriggerEntered> onTriggerEntered { get; }
    public List<OnTriggerExited> onTriggerExited { get; }

    public void RegisterListener(OnTriggerEntered onTriggerEntered, OnTriggerExited onTriggerExited);
}
