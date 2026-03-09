using UnityEngine;
using UnityEngine.UI;
using UGC.Drag.Interfaces;
using UGC.Drag.Models;
using UGC.Drag.Runtime;

namespace UGC.Drag.Core
{
    public sealed class DefaultDragVisualFactory : MonoBehaviour, IDragVisualFactory
    {
        public RectTransform CreateVisual(DragContext context, Transform visualRoot)
        {
            if (context?.Source == null || visualRoot == null)
            {
                return null;
            }

            var sourceRect = context.Source.transform as RectTransform;
            if (sourceRect == null)
            {
                return null;
            }

            var clone = Instantiate(sourceRect.gameObject, visualRoot);
            clone.name = $"{sourceRect.gameObject.name}_DragVisual";

            foreach (var dragSource in clone.GetComponentsInChildren<DragSource>(true))
            {
                Destroy(dragSource);
            }

            foreach (var dropTarget in clone.GetComponentsInChildren<DropTarget>(true))
            {
                Destroy(dropTarget);
            }

            DisableRaycastRecursively(clone.transform);

            var cloneRect = clone.transform as RectTransform;
            if (cloneRect != null)
            {
                cloneRect.SetAsLastSibling();
            }

            return cloneRect;
        }

        private static void DisableRaycastRecursively(Transform root)
        {
            if (root == null)
            {
                return;
            }

            foreach (var graphic in root.GetComponentsInChildren<Graphic>(true))
            {
                graphic.raycastTarget = false;
            }

            var cg = root.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = root.gameObject.AddComponent<CanvasGroup>();
            }

            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
    }
}

