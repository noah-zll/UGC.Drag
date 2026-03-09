using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UGC.Drag.Core;
using UGC.Drag.Interfaces;
using UGC.Drag.Models;

namespace UGC.Drag.Runtime
{
    public sealed class DragService : MonoBehaviour
    {
        public Transform visualRoot;
        public GraphicRaycaster raycaster;
        public MonoBehaviour visualFactory;
        public bool useEventSystemRaycastAll = true;

        public bool IsDragging => m_Context != null;

        public DragContext LastContext => m_LastContext;

        private readonly List<RaycastResult> m_RaycastResults = new List<RaycastResult>(32);
        private DragContext m_Context;
        private DragContext m_LastContext;
        private RectTransform m_Visual;
        private DragVisualFollower m_Follower;
        private DropTarget m_Hover;

        private void Awake()
        {
            if (raycaster == null)
            {
                raycaster = GetComponentInParent<GraphicRaycaster>();
            }

            if (visualRoot == null)
            {
                visualRoot = transform;
            }
        }

        public void BeginDrag(DragContext context)
        {
            if (context == null || context.Source == null)
            {
                return;
            }

            m_Context = context;
            m_LastContext = context;

            CreateVisual(context);
            UpdateHover(context.EventData);
            UpdateVisualPosition(context.EventData);
        }

        public void UpdateDrag(PointerEventData eventData)
        {
            if (m_Context == null)
            {
                return;
            }

            m_Context.EventData.position = eventData.position;

            UpdateHover(eventData);
            UpdateVisualPosition(eventData);
        }

        public DragResult EndDrag(PointerEventData eventData)
        {
            if (m_Context == null)
            {
                return new DragResult(false, null, "NoActiveDrag");
            }

            UpdateHover(eventData);

            var target = m_Hover;
            if (target == null)
            {
                Cleanup();
                return new DragResult(false, null, "NoTarget");
            }

            if (!target.CanAccept(m_Context))
            {
                Cleanup();
                return new DragResult(false, target, "Rejected");
            }

            target.OnDrop(m_Context);

            Cleanup();
            return new DragResult(true, target, null);
        }

        private void CreateVisual(DragContext context)
        {
            DestroyVisual();

            var root = visualRoot != null ? visualRoot : transform;
            var factory = visualFactory as IDragVisualFactory;
            if (factory == null)
            {
                factory = GetComponent<IDragVisualFactory>();
            }

            if (factory == null)
            {
                var fallback = GetComponent<DefaultDragVisualFactory>();
                if (fallback == null)
                {
                    fallback = gameObject.AddComponent<DefaultDragVisualFactory>();
                }

                factory = fallback;
            }

            m_Visual = factory.CreateVisual(context, root);
            if (m_Visual == null)
            {
                return;
            }

            var follower = m_Visual.GetComponent<DragVisualFollower>();
            if (follower == null)
            {
                follower = m_Visual.gameObject.AddComponent<DragVisualFollower>();
            }

            var rootRect = root as RectTransform;
            if (rootRect == null)
            {
                rootRect = (transform as RectTransform);
            }

            follower.Initialize(rootRect, context.EventData.pressEventCamera);
            m_Follower = follower;
        }

        private void UpdateVisualPosition(PointerEventData eventData)
        {
            if (m_Follower == null)
            {
                return;
            }

            m_Follower.SetPosition(eventData);
        }

        private void UpdateHover(PointerEventData eventData)
        {
            var target = RaycastTarget(eventData);
            if (ReferenceEquals(target, m_Hover))
            {
                m_Context.CurrentHover = m_Hover;
                return;
            }

            if (m_Hover != null)
            {
                m_Hover.OnDragExit(m_Context);
            }

            m_Hover = target;
            m_Context.CurrentHover = target;

            if (m_Hover != null)
            {
                m_Hover.OnDragEnter(m_Context);
            }
        }

        private DropTarget RaycastTarget(PointerEventData eventData)
        {
            m_RaycastResults.Clear();
            if (useEventSystemRaycastAll)
            {
                var es = EventSystem.current;
                if (es != null)
                {
                    es.RaycastAll(eventData, m_RaycastResults);
                }
                else if (raycaster != null)
                {
                    raycaster.Raycast(eventData, m_RaycastResults);
                }
            }
            else if (raycaster != null)
            {
                raycaster.Raycast(eventData, m_RaycastResults);
            }

            for (int i = 0; i < m_RaycastResults.Count; i++)
            {
                var go = m_RaycastResults[i].gameObject;
                if (go == null)
                {
                    continue;
                }

                var target = go.GetComponentInParent<DropTarget>();
                if (target == null || !target.isActiveAndEnabled)
                {
                    continue;
                }

                if (!target.CanAccept(m_Context))
                {
                    continue;
                }

                return target;
            }

            return null;
        }

        private void Cleanup()
        {
            if (m_Hover != null)
            {
                m_Hover.OnDragExit(m_Context);
            }

            m_Hover = null;
            m_Context = null;
            DestroyVisual();
        }

        private void DestroyVisual()
        {
            if (m_Visual != null)
            {
                Destroy(m_Visual.gameObject);
            }

            m_Visual = null;
            m_Follower = null;
        }
    }
}

