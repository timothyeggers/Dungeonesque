using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(VisualDetector))]
public class ExperimentalMelee : MonoBehaviour
{
    public Collider target => inView[targetIndex];

    [SerializeField] private GameObject parent;

    private VisualDetector detector;
    private int targetIndex = 0;
    private List<Collider> inView = new List<Collider>() { null };

    private void Start()
    {
        detector = GetComponent<VisualDetector>();
        detector.RegisterListener(OnEnemyEntered, OnEnemyExited);
    }

    public void OnEnemyEntered(Collider other)
    {
        Debug.Log("Enemy entered view..." + other.name);
        inView.Add(other);
    }

    public void OnEnemyExited(Collider other)
    {
        Debug.Log("Enemy exited view..." + other.name);
        if (inView.Contains(other)) inView.Remove(other);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetIndex++;

            if (targetIndex >= inView.Count) targetIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Q)) {
            targetIndex--;

            if (targetIndex < 0) targetIndex = inView.Count - 1;
        }
        

        if (target)
        {
            parent.transform.forward = (target.transform.position - parent.transform.position).normalized;
            Debug.DrawLine(parent.transform.position, target.transform.position, Color.red);
        } else
        {
            parent.transform.forward = LookAtMouse();
        }
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
