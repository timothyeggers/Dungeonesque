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

        if (initiatedAttack)
        {
            attackStartPos = mousePosition;
            angleSelector.Reset();
        }

        if (holdingDownAttack)
        {
            angleSelector.Move(attackStartPos, mousePosition);
            attackEndPos = mousePosition;
            
            switch (meleeParameters)
            {
                case SwingMeleeParameters swing:
                    armPivot.transform.eulerAngles = characterPivot.transform.eulerAngles + GetRotationForSwing(swing, attackStartPos, attackEndPos);
                    break;
                default:
                    break;
            }
            
        }

        if (releaseAttack)
        {
            attacking = true;
            slicer.enabled = true;

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
            swingPivot.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, -180, 0), attackDt);
            attackDt += Time.deltaTime * 2f;

            if (attackDt > meleeParameters.DurationOfAttack)
            {
                attacking = false;
                slicer.enabled = false;
                attackDt = 0f;
                swingPivot.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

    //todo : implement function for getrotationstab, which modifies x and y position, and x rotation slighjtly (to point at target)


    /// <summary>
    /// Rotates X, Y, Z to have the swingPivot appropiately positioned for a swing attack.  
    /// Rotation is determined by the direction from origin to the destination on a 2D plane.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public Vector3 GetRotationForSwing(SwingMeleeParameters swing, Vector2 origin, Vector2 destination)
    {
        // dont touch this
        var point = destination - origin;
        float angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
        int sign = Mathf.Abs(angle) > 90 ? -1 : 1;
        Debug.Log(point);
        // y rotation
        var xMagnitude = point.x / angleSelector.MaxDistanceFromPivot;
        xMagnitude = Mathf.Clamp(xMagnitude, -1, 1);

        // x rotation
        var yMagnitude = point.y / angleSelector.MaxDistanceFromPivot;
        yMagnitude = Mathf.Clamp(yMagnitude, -1, 1);
        Debug.Log("Magnitude " + new Vector2(xMagnitude, yMagnitude));
        var yRot = xMagnitude * (swing.MaxHorizontalRotation / 2);
        var xRot = -yMagnitude * (swing.MaxVerticalRotation / 2);
        var zRot = yMagnitude * 90;

        var targetRot = new Vector3(xRot, yRot, zRot);

        return targetRot;
    }
}
/*
 * public Collider target
    {
        get
        {
            if (targetIndex < inView.Count)
                return inView[targetIndex];
            targetIndex = 0;
            state = CharacterSwivel.FREE;
            return null;
        }
    }
    [SerializeField] private GameObject parent;
    [SerializeField] private Transform armPivot;
    [SerializeField] private Transform swingRotation;
    [SerializeField] private VisualDetector detector;
    [SerializeField] private float horizontalRotation = 150f;
    [SerializeField] private float verticalRotation = 80f;
    
    [SerializeField] private MeshCollider hitCollider;
    [SerializeField] private float selectorRadiusSetting = 150f;
    
    private int targetIndex = 0;
    private List<Collider> inView = new List<Collider>() { null };

    private CharacterSwivel state = CharacterSwivel.FREE;
    private Vector3 lockDirection = Vector2.zero;

    private Vector3 startRotation;
    private Vector3 mouseLockOrigin;
    

    private void Start()
    {
        detector.RegisterListener(OnEnemyEntered, OnEnemyExited);
        startRotation = armPivot.eulerAngles;
        hitCollider.enabled = false;
    }

    public void OnEnemyEntered(Collider other)
    {
        inView.Add(other);
    }

    public void OnEnemyExited(Collider other)
    {
        if (inView.Contains(other))
            inView.Remove(other); 
    }

    public Collider FindClosestTarget()
    {
        Collider target = null;
        float distanceToTarget = Mathf.Infinity;
        foreach (Collider enemy in inView)
        {
            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < distanceToTarget)
            {
                target = enemy;
                distanceToTarget = distance;
            }
        }

        return target;
    }

    private void Update_TargetLock()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetIndex++;
            
            if (targetIndex >= inView.Count) targetIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            targetIndex--;

            if (targetIndex < 0) targetIndex = inView.Count - 1;
        }

        if (target)
        {
            state = CharacterSwivel.TARGET_LOCK;
            // check if our mouse is trying to break off target
            var mousePos = Input.mousePosition;
            var targetOnScreen = Camera.main.WorldToScreenPoint(target.transform.position);
            
            if (Vector2.Distance(mousePos, targetOnScreen) > Screen.width / 3)
            {
                state = CharacterSwivel.FREE;
                targetIndex = 0;
            }
        }
    }

    private void Update_FixedLock()

    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (state == CharacterSwivel.FIXED) {
                state = CharacterSwivel.FREE;
                return;
            }
            state = CharacterSwivel.FIXED;
            lockDirection = LookAtMouse();
            mouseLockOrigin = Input.mousePosition;
        }
    }

    float swingTime = 0.75f;
    float swingDt = 0f;
    bool swinging = false;

    private void Update()
    {
        Update_TargetLock();
        Update_FixedLock();

        


        switch (state)
        {
            case CharacterSwivel.FREE:
                parent.transform.forward = LookAtMouse();
                break;
            case CharacterSwivel.FIXED:
                parent.transform.forward = lockDirection;
                Update_SwordRotation(mouseLockOrigin, Input.mousePosition);
                break;
/*            case CharacterSwivel.TARGET_LOCK:
                var forward = (target.transform.position - parent.transform.position).normalized;
                forward.y = 0f;
                parent.transform.forward = forward;
                Update_SwordRotation();
                
                Debug.DrawLine(parent.transform.position, target.transform.position, Color.red);
                break;
        }
        

        if (Input.GetMouseButtonDown(0) && swinging == false)
        {
            swinging = true;
            hitCollider.enabled = true;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            armPivot.eulerAngles = Vector3.zero;
        }

        if (swinging)
        {
            swingRotation.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, -180, 0), swingDt);
            swingDt += Time.deltaTime * 2f;

            if (swingDt > swingTime)
            {
                swinging = false;
                hitCollider.enabled = false;
                swingDt = 0f;
                swingRotation.localEulerAngles = Vector3.zero;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float radius = state != AttackState.FREE ? 1 : 0;
        Gizmos.DrawSphere(mouseLockOrigin, radius);
    }

    private void Update_SwordRotation(Vector2 origin, Vector2 destination)
    {
        // dont touch this
        var point = destination - origin;
        float angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
        int sign = Mathf.Abs(angle) > 90 ? -1 : 1;

        // y rotation
        var xMagnitude = point.x / selectorRadiusSetting;
        xMagnitude = Mathf.Clamp(xMagnitude, -1, 1);

        // x rotation
        var xMagnitude = point.y / selectorRadiusSetting;
        xMagnitude = Mathf.Clamp(xMagnitude, -1, 1);

        var yRot = xMagnitude * (horizontalRotation / 2);
        var xRot = -xMagnitude * (verticalRotation / 2);
    
        var targetRot = new Vector3(xRot, yRot, 0);

        armPivot.eulerAngles = parent.transform.eulerAngles + targetRot;
        if (state == AttackState.FREE)
        {
            armPivot.eulerAngles += startRotation;
        }

        Debug.DrawLine(origin, destination, Color.red);
    }

    Vector2 DirectionToMouse()
    {
        var playerPosInScreen = Camera.main.WorldToScreenPoint(transform.position);
        var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var forward = (mousePos - new Vector2(playerPosInScreen.x, playerPosInScreen.y)).normalized;
        return forward;
    }

    Vector3 LookAtMouse()
    {
        var forward = DirectionToMouse();
        var playerForward = new Vector3(forward.x, 0, forward.y);
        return playerForward;
    }
    /*
        void Update()
        {
            *//*        var x = Input.mousePosition.x - (Screen.width / 2)  * transform.forward.z;
                    var y = Input.mousePosition.y - (Screen.height / 2);
                    var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    var forward = (mousePos - ScreenMiddle).normalized;
                    var playerPosInScreen = Camera.main.WorldToScreenPoint(transform.position);
                    forward = (mousePos - new Vector2(playerPosInScreen.x, playerPosInScreen.y)).normalized;
                    var playerForward = new Vector3(forward.x, 0, forward.y);
                    var playerForwardRight = Quaternion.Euler(0, transform.eulerAngles.y, 0) * playerForward;
                    var playerForwardLeft = Quaternion.Euler(0, transform.eulerAngles.y, 0) * playerForward;
                    *//*
            IntersectPlayerView();

            transform.forward = LookAtMouse();
            var x = Input.GetAxis("Horizontal") * 5f * Time.deltaTime;
            var z = Input.GetAxis("Vertical") * 5f * Time.deltaTime;
            transform.position += new Vector3(x, 0,z );
        }

        private void IntersectPlayerView()
        {
            float fovRads = cleaveWidth * Mathf.Deg2Rad;
            float resolution = cleaveWidth / 5;
            fovRads /= resolution;

            List<RaycastHit> rayHits = new List<RaycastHit>{ };

            for (int i = (int) -(resolution / 2); i <= (resolution / 2); i++)
            {
                float angle = fovRads * i;
                var direction = DirectionFrom(angle);

                direction = Quaternion.Euler(0, 0, 0) * direction;

                var ray = new Ray(transform.position, direction);
                var rayDist = range;
                var color = Color.red;
                if (Physics.Raycast(ray, out var hit, range, monitorLayer))
                {
                    color = Color.green;
                    rayDist = hit.distance;
                    rayHits.Add(hit);
                }

                Debug.DrawRay(ray.origin, ray.direction * rayDist, color);
            }

            float closestEnemyPos = Mathf.Infinity;
            RaycastHit? closestEnemy = null;

            foreach (var hit in rayHits) {
                if (closestEnemyPos > hit.distance)
                {
                    if (closestEnemy is RaycastHit enemy)
                    {
                        var distance = Vector3.Distance(hit.transform.position, enemy.transform.position);
                        Debug.Log(distance);
                        Debug.Log(enemy.collider.name + " " + hit.collider.name);
                        // should update closest enemy
                        if ( distance < cleaveDistanceWindow && hit.collider != enemy.collider)
                        {
                            // should cleave
                            Debug.Log("Should cleave..");
                        }

                    }

                    closestEnemy = hit;
                    closestEnemyPos = hit.distance;
                    *//*Debug.Log(closestEnemyPos);*//*
                }
            }

            if (closestEnemy is RaycastHit enemyA)
            {
                transform.forward = (enemyA.point - transform.position).normalized;
            } else
            {
                transform.forward = LookAtMouse();
            }

        }
}*/
