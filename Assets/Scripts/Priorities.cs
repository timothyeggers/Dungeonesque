using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;


public enum Priorities
{
    Aggressor,
    Visual,
    Audio,
    Prey,
    Other
}

public interface IPriority
{
    /// <summary>
    /// Weight represents the priority of the target.  
    /// In PriorityController it's used to determine the time in seconds before it's no longer a priority.
    /// </summary>
    public float weight { get; }

    public Priorities type { get; }
}

[Serializable]
public struct DemoSoundTarget : IPriority
{
    public Priorities type => Priorities.Audio;

    public float weight => 3f;
}

[Serializable]
public struct DemoVisualTarget : IPriority
{
    public Priorities type => Priorities.Visual;

    public float weight => 4f;
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