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
    Prey
}

// Base interface for all objects detected by tower
public interface ITarget
{
    public Priorities Priority { get; }
}

[Serializable]
public struct SoundTarget : ITarget
{
    public Priorities Priority => Priorities.Audio;

    public float weight;
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
}