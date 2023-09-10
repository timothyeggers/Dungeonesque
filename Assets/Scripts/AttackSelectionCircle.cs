using UnityEngine;
using static UnityEngine.UI.Image;

public class AttackSelectionCircle: ScriptableObject
{
    public Texture2D normalMap; 


/*    public void GetSomething(Vector2 origin, Vector2 destination)
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
    }*/
}