using UnityEngine;
using UGC.Drag.Interfaces;
using UGC.Drag.Models;

namespace UGC.Drag.Runtime
{
    public sealed class DropTarget : MonoBehaviour, IDropTarget
    {
        public MonoBehaviour handler;

        public event System.Action<DragContext> onDragEnter;
        public event System.Action<DragContext> onDragExit;
        public event System.Action<DragContext> onDrop;

        public bool CanAccept(DragContext context)
        {
            var h = handler as IDropTarget;
            return h == null || h.CanAccept(context);
        }

        public void OnDragEnter(DragContext context)
        {
            var h = handler as IDropTarget;
            h?.OnDragEnter(context);
            onDragEnter?.Invoke(context);
        }

        public void OnDragExit(DragContext context)
        {
            var h = handler as IDropTarget;
            h?.OnDragExit(context);
            onDragExit?.Invoke(context);
        }

        public void OnDrop(DragContext context)
        {
            var h = handler as IDropTarget;
            h?.OnDrop(context);
            onDrop?.Invoke(context);
        }
    }
}

