using UnityEngine.EventSystems;

namespace UGC.Drag.Models
{
    public sealed class DragContext
    {
        public DragContext(Runtime.DragSource source, object payload, PointerEventData eventData)
        {
            Source = source;
            Payload = payload;
            EventData = eventData;
        }

        public Runtime.DragSource Source { get; }

        public object Payload { get; }

        public PointerEventData EventData { get; }

        public Runtime.DropTarget CurrentHover { get; internal set; }
    }
}

