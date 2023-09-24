using Dungeonesque.Core;
using UnityEngine;

namespace Dungeonesque.Weapons
{
    /// <summary>
    ///     MeleeController uses an AngleSelector and supplies positional input to produce a final attack angle.
    /// </summary>
    public class MeleeController : MonoBehaviour
    {
        [SerializeField] private MeleeParameters meleeParameters;
        [SerializeField] private RotateTool angleSelector;

        [SerializeField] private GameObject characterPivot;
        [SerializeField] private GameObject armPivot;
        [SerializeField] private GameObject swingPivot;

        [Tooltip(
            "If the distance from the start of the attack and the release of the attack is less than this, we'll just assume to auto-combo.")]
        private readonly float minimumDistanceForNewAttackAngle = 10f;

        private int attackDirection = 1;

        private float attackDt;
        private bool attacking;

        private Vector2 attackStartPos = Vector2.zero;

        private bool preparingAttack;

        // move rotation
        private Vector3 targetRotation;

        private void Update()
        {
            var initiatedAttack = Input.GetMouseButtonDown(0);
            var holdingDownAttack = Input.GetMouseButton(0);
            var releaseAttack = Input.GetMouseButtonUp(0);

            var mousePosition = (Vector2)Input.mousePosition;

            if (initiatedAttack && !attacking)
            {
                attackStartPos = mousePosition;
                preparingAttack = true;
            }

            if (holdingDownAttack && !attacking)
            {
                var speed = meleeParameters.Agility;
                angleSelector.Move(attackStartPos, mousePosition);

                switch (meleeParameters)
                {
                    case SwingMeleeParameters swing:
                        targetRotation = characterPivot.transform.eulerAngles +
                                         angleSelector.TranslatePositionToRotation(swing.MaxHorizontalRotation,
                                             swing.MaxVerticalRotation);
                        break;
                }

                armPivot.transform.rotation = Quaternion.Lerp(armPivot.transform.rotation,
                    Quaternion.Euler(targetRotation),
                    speed * Time.deltaTime);
            }

            if (releaseAttack && !attacking)
            {
                attacking = true;
                preparingAttack = false;

                if (Vector2.Distance(mousePosition, attackStartPos) < minimumDistanceForNewAttackAngle)
                {
                    // do standard combo attack, forget the selected angle.
                }
            }

            if (attacking)
            {
                attackDt += Time.deltaTime * meleeParameters.AttackSpeed * attackDirection;
                attackDt = Mathf.Clamp(attackDt, 0, meleeParameters.AttackSpeed);

                if (attackDt == 0)
                {
                    attackDt = 0f;
                    attacking = false;
                    attackDirection = 1;
                }

                if (attackDt == meleeParameters.AttackSpeed)
                {
                    attackDirection = -1;
                    attackDt = meleeParameters.AttackSpeed;
                }

                switch (meleeParameters)
                {
                    case SwingMeleeParameters swing:
                        swingPivot.transform.localEulerAngles =
                            Vector3.Lerp(Vector3.zero, new Vector3(0, 180, 0), attackDt);
                        break;
                }
            }
        }

        public bool IsAttacking()
        {
            return attacking;
        }

        public bool IsPreparingAttack()
        {
            return preparingAttack;
        }
    }
}