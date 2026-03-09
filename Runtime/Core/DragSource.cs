using UnityEngine;
using UnityEngine.EventSystems;
using UGC.Drag.Interfaces;
using UGC.Drag.Models;

namespace UGC.Drag.Runtime
{
    public sealed class DragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public DragService service;
        public bool hideWhileDragging = true;
        public MonoBehaviour handler;
        public Object payloadAsset;

        public event System.Action<DragContext> onDragStarted;
        public event System.Action<DragContext, DragResult> onDragEnded;

        private CanvasGroup m_CanvasGroup;
        private float m_OriginalAlpha = 1f;

        private void Awake()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
            if (m_CanvasGroup != null)
            {
                m_OriginalAlpha = m_CanvasGroup.alpha;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (service == null)
            {
                return;
            }

            var h = handler as IDragSource;
            if (h != null && !h.CanBeginDrag(eventData))
            {
                return;
            }

            var payload = h != null ? h.BuildPayload() : (payloadAsset != null ? payloadAsset : this);
            var context = new DragContext(this, payload, eventData);

            h?.OnDragStarted(context);
            onDragStarted?.Invoke(context);
            service.BeginDrag(context);

            if (hideWhileDragging)
            {
                EnsureCanvasGroup();
                m_OriginalAlpha = m_CanvasGroup.alpha;
                m_CanvasGroup.alpha = 0f;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            service?.UpdateDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (service == null)
            {
                return;
            }

            var result = service.EndDrag(eventData);

            if (hideWhileDragging && m_CanvasGroup != null)
            {
                m_CanvasGroup.alpha = m_OriginalAlpha;
            }

            var context = service.LastContext;
            if (context != null)
            {
                var h = handler as IDragSource;
                h?.OnDragEnded(context, result);
                onDragEnded?.Invoke(context, result);
            }
        }

        private void EnsureCanvasGroup()
        {
            if (m_CanvasGroup != null)
            {
                return;
            }

            m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
            m_OriginalAlpha = m_CanvasGroup.alpha;
        }
    }
}
