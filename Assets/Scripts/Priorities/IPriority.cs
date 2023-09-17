using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PriorityType
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
    /// Weight represents the priority of the activeTarget.  
    /// In PriorityController it's used to determine the time in seconds before it's no longer a priority.
    /// </summary>
    public int UUID { get; }

    public float weight { get; }

    public PriorityType type { get; }
}
