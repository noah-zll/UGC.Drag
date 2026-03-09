using UnityEngine;
using UnityEngine.EventSystems;

namespace UGC.Drag.Core
{
    public sealed class DragVisualFollower : MonoBehaviour
    {
        private RectTransform m_Root;
        private RectTransform m_RectTransform;
        private Camera m_Camera;

        public void Initialize(RectTransform root, Camera camera)
        {
            m_Root = root;
            m_Camera = camera;
            m_RectTransform = transform as RectTransform;
        }

        public void SetPosition(PointerEventData eventData)
        {
            if (m_Root == null || m_RectTransform == null)
            {
                return;
            }

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Root, eventData.position, m_Camera, out var localPoint))
            {
                return;
            }

            m_RectTransform.anchoredPosition = localPoint;
        }
    }
}

