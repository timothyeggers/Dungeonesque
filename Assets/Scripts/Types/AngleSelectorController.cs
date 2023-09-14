using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;


/// <summary>
/// AngleSelectorController has an indicator that can be dragged around a pivot.
/// </summary>
public class AngleSelectorController : MonoBehaviour
{
    public float MaxDistanceFromPivot => maxDistanceFromPivot;
    public float DistanceFromPivot => Vector2.Distance(pivot, indicator.transform.position);
    public Vector2 NormalizedPositionFromPivot => (Vector2) indicator.transform.position - pivot;

    [Tooltip("Indicator is the point moving within the wheel.")]
    [SerializeField] private Rigidbody2D indicator;

    [SerializeField] private float maxDistanceFromPivot = 0.70f;
    
    private Vector2 pivot => transform.position;
    private Vector2 startPos; // indiciator start position

    public float IndicatorRadius { 
        get 
        {
            if (indicator.TryGetComponent<CircleCollider2D>(out var collider))
            {
                return collider.radius;
            }
            return 0.01f;
        }
    }

    private void Start()
    {
        startPos = indicator.transform.localPosition;
    }

    public void Reset()
    {
        indicator.transform.localPosition = startPos;
    }
    
    public void Move(Vector2 origin, Vector2 destination) 
    {
        var point = destination - origin;

        // normalize point magnitude to the scale of the wheel. 
        point /= 100f;
        point = Vector2.ClampMagnitude(point, maxDistanceFromPivot);
        point += pivot;
        
        var move = Physics2D.OverlapCircle(point, IndicatorRadius, 1 << 9);
        
        // kind of weird, but basically we want the indicator to move between colliders if the magnitude is long enough
        // otherwise we want the indicator to slide against the collider.
        // ensure continuous detection is enabled for the indicator rigidbody2d.
        if (move == null) indicator.transform.position = point;
        else indicator.MovePosition(point);
    }
}
