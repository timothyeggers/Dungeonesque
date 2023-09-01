﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class AudioTrigger : MonoBehaviour, IPriority
{
    [SerializeField]
    private GameEventSO onAudioTriggered;

    public float volume = 1f;

    public float weight => volume;

    public Priorities type => Priorities.Audio;

    public void Trigger()
    {
        onAudioTriggered?.Raise(this);
    }
}