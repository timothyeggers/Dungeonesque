using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;



[Serializable]
public struct DemoVisualTarget : IPriority
{
    public int uuid;

    public int UUID => uuid;

    public PriorityType type => PriorityType.Visual;

    public float weight => 4f;

    public DemoVisualTarget(int uuid)
    {
        this.uuid = uuid;
    }
}
/*
// Base interface for all objects detected by tower
public interface ITarget
{
    public Priorities Priority { get; }
}



[Serializable]
public struct VisualTarget : ITarget
{
    public Priorities Priority => Priorities.Visual;

    public float weight;
}

[Serializable]
public struct VisualTargetConfirm : ITarget
{
    public Priorities Priority => Priorities.Aggressor;

    public float weight;
}*/