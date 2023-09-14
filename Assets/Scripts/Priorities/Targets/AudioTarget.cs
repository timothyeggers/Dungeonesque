using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct DemoSoundTarget : IPriority
{
    public int uuid;

    public int UUID => uuid;

    public PriorityType type => PriorityType.Audio;

    public float weight => 3f;

    public DemoSoundTarget(int uuid)
    {
        this.uuid = uuid;
    }
}
