using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using EzySlice;


/// <summary>
/// MeleeController uses an AngleSelector and supplies positional input to produce a final attack angle.
/// </summary>
public class MeleeController : MonoBehaviour
{
    [SerializeField] private MeleeParameters meleeParameters;
    [SerializeField] private AngleSelectorController angleSelector;
    
    [SerializeField] private GameObject characterPivot;
    [SerializeField] private GameObject armPivot;
    [SerializeField] private GameObject swingPivot;

    [SerializeField] private BladeSlicer slicer;
    
    [Tooltip("If the distance from the start of the attack and the release of the attack is less than this, we'll just assume to auto-combo.")]
    private float minimumDistanceForNewAttackAngle = 10f;

    // if it's zero then an attack isn't initiated.
    private Vector2 attackStartPos = Vector2.zero;
    private Vector2 attackEndPos = Vector2.zero;
    private float attackDt = 0f;
    private bool attacking = false;
    private bool preparingAttack = false;

    // move rotation
    private Vector3 targetRotation;

    public bool IsAttacking() => attacking;
    public bool IsPreparingAttack() => preparingAttack;

    private void OnEnable()
    {
        slicer.enabled = false;
    }

    Vector2 prevMousePosition;
    private void Update()
    {
        var initiatedAttack = Input.GetMouseButtonDown(0);
        var holdingDownAttack = Input.GetMouseButton(0);
        var releaseAttack = Input.GetMouseButtonUp(0);

        var mousePosition = (Vector2) Input.mousePosition;

        if (initiatedAttack && !attacking)
        {
            attackStartPos = mousePosition;
            preparingAttack = true;
        }

        if (holdingDownAttack && !attacking)
        {
            float speed = meleeParameters.Agility;
            angleSelector.Move(attackStartPos, mousePosition);
            attackEndPos = mousePosition;
            
            switch (meleeParameters)
            {
                case SwingMeleeParameters swing:
                    targetRotation = characterPivot.transform.eulerAngles + 
                        angleSelector.TranslatePositionToRotation(swing.MaxHorizontalRotation, swing.MaxVerticalRotation);
                    break;
                default:
                    break;
            }
            
            armPivot.transform.rotation = Quaternion.Lerp(armPivot.transform.rotation, Quaternion.Euler(targetRotation), speed * Time.deltaTime);
        }

        if (releaseAttack && !attacking)
        {
            attacking = true;
            preparingAttack = false;
            slicer.enabled = true;
            Debug.Log("Starting rotation of swingPivot" + swingPivot.transform.localEulerAngles);

            if (Vector2.Distance(mousePosition, attackStartPos) < minimumDistanceForNewAttackAngle)
            {
                // do standard combo attack, forget the selected angle.
            }
            else
            {
                
            }
        }

        if (attacking)
        {
            var direction = slicer.enabled ? 1 : -1;
            attackDt += Time.deltaTime * meleeParameters.AttackSpeed * direction;

            if (attackDt >= 1)
            {
                slicer.enabled = false;
            }

            if (attackDt <= 0)
            {
                attackDt = 0f;
                attacking = false;
            }

            switch (meleeParameters)
            {
                case SwingMeleeParameters swing:
                    swingPivot.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 180, 0), attackDt);
                    break;
                default:
                    break;
            }
        }
    }
}
