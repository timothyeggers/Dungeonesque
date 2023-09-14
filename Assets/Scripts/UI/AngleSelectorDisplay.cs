using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// AngleSelectorDisplay displays whatevers going on with AngleSelectorController
/// </summary>
public class AngleSelectorDisplay : MonoBehaviour
{
    [SerializeField] private AngleSelectorController controller;

    [SerializeField] private Image wheel;
    [SerializeField] private Image indicator;

    private Vector2 startPos;

    private void OnEnable()
    {
        startPos = indicator.transform.localPosition;
    }

    private void Update()
    {
        // we want to offset the indicator by the direction (with magnitude) from the pivot
        // and then multiple that magnitude by the width of the wheel to scale it for the canvas.
        var offset = controller.DirectionFromPivot * controller.Radius;
        offset *= wheel.rectTransform.rect.width;
        indicator.transform.localPosition = startPos + offset;
    }

}

/*
public void SetWheel(Texture2D texture)
{
    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    wheel.sprite = sprite;
    indicator.gameObject.transform.position = startPos;
}

public void SetIndicatorOffset(Vector2 offset)
{
    var point = startPos + Vector2.ClampMagnitude(offset, wheel.rectTransform.rect.width / 2);
    indicator.gameObject.transform.position = point;
}*/