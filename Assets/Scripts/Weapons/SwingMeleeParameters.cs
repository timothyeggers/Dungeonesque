
using UnityEngine;

[CreateAssetMenu(fileName = "Default Melee Parameters", menuName = "Scriptable Objects/Melee Parameters")]
public class SwingMeleeParameters : MeleeParameters
{
    [Header("Swing attack parameters.")]
    public float MaxHorizontalRotation = 275f;
    public float MaxVerticalRotation = 80f;
}