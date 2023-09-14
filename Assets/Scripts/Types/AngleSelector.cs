using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;


/// <summary>
/// AngleSelector is used determining the valid angle of an attack, given a normal map texture.
/// </summary>
[CreateAssetMenu(fileName = "Default Weapon Angles", menuName = "Scriptable Objects/Weapon Angles")]
public class AngleSelector : ScriptableObject
{
    [Tooltip("This texture will determine if a point is in-bounds if it isn't over a black area.")]
    public Texture2D wheel;

    public float Radius => wheel.width / 2;

    public Color GetPixelInTexture(Vector2 origin, Vector2 destination)
    {
        var halfWidth = wheel.width / 2;
        var halfHeight = wheel.height / 2;

        var direction = destination - origin;
        var x = direction.x / halfWidth;
        var y = direction.y / halfHeight;

        var xInNormal = halfWidth + (halfWidth * x);
        xInNormal = Mathf.Clamp(xInNormal, 0, wheel.width);

        var yInNormal = halfHeight + (halfHeight * y);
        yInNormal = Mathf.Clamp(yInNormal, 0, wheel.height);

        return wheel.GetPixel((int)xInNormal, (int)-yInNormal);
    }

    public bool IsPointInBounds(Vector2 origin, Vector2 destination)
    {
        var pixel = GetPixelInTexture (origin, destination);
        if (pixel == Color.black) return false;
        return true;
    }

    public Vector2 GetClosestPointInBounds(Vector2 origin, Vector2 destination)
    {
        if (origin == destination) return origin;

        if (IsPointInBounds(origin, destination)) return destination;

        destination *= 0.99f;
        Debug.Log(destination);

        return GetClosestPointInBounds(origin, destination);
    }
}
