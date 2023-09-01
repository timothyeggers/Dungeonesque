using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField]
    private GameEventSO onAudioTriggered;

    public float volume = 1f;

    public void Trigger()
    {
        onAudioTriggered?.Raise(this);
    }
}