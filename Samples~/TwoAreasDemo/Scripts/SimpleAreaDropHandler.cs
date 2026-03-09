using UnityEngine;
using UGC.Drag.Interfaces;
using UGC.Drag.Models;

namespace UGC.Drag.Samples
{
    public sealed class SimpleAreaDropHandler : MonoBehaviour, IDropTarget
    {
        public RectTransform contentRoot;

        public bool CanAccept(DragContext context)
        {
            return true;
        }

        public void OnDragEnter(DragContext context)
        {
        }

        public void OnDragExit(DragContext context)
        {
        }

        public void OnDrop(DragContext context)
        {
            var rt = context.Source.transform as RectTransform;
            if (rt == null)
            {
                return;
            }

            if (contentRoot != null)
            {
                rt.SetParent(contentRoot, worldPositionStays: false);
                rt.SetAsLastSibling();
                rt.anchoredPosition = Vector2.zero;
                rt.localScale = Vector3.one;
            }
        }
    }
}

