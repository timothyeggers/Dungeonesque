using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using EzySlice;


enum CharacterSwivel
{
    FREE,
    FIXED,
    TARGET_LOCK
}

public class ExperimentalMelee : MonoBehaviour
{
    public Collider target
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
    [SerializeField] private Transform armRotation;
    [SerializeField] private Transform swingRotation;
    [SerializeField] private VisualDetector detector;
    [SerializeField] private float horizontalRotation = 150f;
    [SerializeField] private float verticalRotation = 80f;
    [SerializeField] private float armLength = 1f;
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
        startRotation = armRotation.eulerAngles;
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
                break;*/
        }
        

        if (Input.GetMouseButtonDown(0) && swinging == false)
        {
            swinging = true;
            hitCollider.enabled = true;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            armRotation.eulerAngles = Vector3.zero;
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
        float radius = state != CharacterSwivel.FREE ? 1 : 0;
        Gizmos.DrawSphere(mouseLockOrigin, radius);
    }

    private void Update_SwordRotation(Vector2 origin, Vector2 destination)
    {
        // dont touch this
        var point = destination - origin;
        float angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
        int sign = Mathf.Abs(angle) > 90 ? -1 : 1;

        // y rotation
        var yMagnitude = point.x / selectorRadiusSetting;
        yMagnitude = Mathf.Clamp(yMagnitude, -1, 1);

        // x rotation
        var xMagnitude = point.y / selectorRadiusSetting;
        xMagnitude = Mathf.Clamp(xMagnitude, -1, 1);

        var yRot = yMagnitude * (horizontalRotation / 2);
        var xRot = -xMagnitude * (verticalRotation / 2);
    
        var targetRot = new Vector3(xRot, yRot, 0);

        armRotation.eulerAngles = parent.transform.eulerAngles + targetRot;
        if (state == CharacterSwivel.FREE)
        {
            armRotation.eulerAngles += startRotation;
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

        }*/
}
