using Dungeonesque.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeonesque.UI
{
    public class RotateToolDisplay : MonoBehaviour
    {
        [SerializeField] private RotateTool controller;

        [SerializeField] private Image wheel;
        [SerializeField] private Image indicator;

        private Vector2 startPos;

        private void Update()
        {
            // we want to offset the indicator by the direction (with magnitude) from the pivot
            // and then multiple that magnitude by the width of the wheel to scale it for the canvas.
            var offset = controller.DirectionFromPivot * controller.Radius;
            offset *= wheel.rectTransform.rect.width;
            indicator.transform.localPosition = startPos + offset;
        }

        private void OnEnable()
        {
            startPos = indicator.transform.localPosition;
        }
    }
}