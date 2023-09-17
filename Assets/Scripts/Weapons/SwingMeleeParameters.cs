
using UnityEngine;

[CreateAssetMenu(fileName = "Default Melee Parameters", menuName = "Scriptable Objects/Melee Parameters")]
public class SwingMeleeParameters : MeleeParameters
{
    [Header("Swing attack parameters.")]
    public float MaxHorizontalRotation = 180f;
    public float MaxVerticalRotation = 180f;
}