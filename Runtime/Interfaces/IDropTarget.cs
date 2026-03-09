using UGC.Drag.Models;

namespace UGC.Drag.Interfaces
{
    public interface IDropTarget
    {
        bool CanAccept(DragContext context);

        void OnDragEnter(DragContext context);

        void OnDragExit(DragContext context);

        void OnDrop(DragContext context);
    }
}

