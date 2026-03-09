using UnityEngine.EventSystems;
using UGC.Drag.Models;

namespace UGC.Drag.Interfaces
{
    public interface IDragSource
    {
        bool CanBeginDrag(PointerEventData eventData);

        object BuildPayload();

        void OnDragStarted(DragContext context);

        void OnDragEnded(DragContext context, DragResult result);
    }
}

