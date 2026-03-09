using UnityEngine;
using UGC.Drag.Models;

namespace UGC.Drag.Interfaces
{
    public interface IDragVisualFactory
    {
        RectTransform CreateVisual(DragContext context, Transform visualRoot);
    }
}
