using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;


public enum DamageType
{
    Slash,
    Blunt,
    Pierce
}

public interface IDamage
{
    public DamageType Type { get; }
    public float Amount { get; }
}

public abstract class MeleeParameters : ScriptableObject, IDamage {

    public float AttackSpeed = 1f;

    public float Agility = 9f;

    #region IDamage declaration
    public DamageType Type => DamageType.Slash;
    public float Amount => 10f;
    #endregion
}

