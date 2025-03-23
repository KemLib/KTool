using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KTool.KInput
{
    public abstract class Mouse
    {
        #region Properties
        private const string NAME_GAMEOBJECT_EVENTSYSTEM = "EventSystem";

        private EventSystem eventSystem;
        private LayerMask layerIgnore;
        private float zoomScale;
        private bool isActive;
        protected bool isDown,
            isMove;
        protected float downTime;
        protected Vector2 positionDown,
            position;

        public delegate void MouseDown(Mouse mouse);
        public delegate void MouseUp(Mouse mouse);
        public delegate void MouseStand(Mouse mouse);
        public delegate void MouseMove(Mouse mouse, Vector2 delta);
        public delegate void MouseScroll(Mouse mouse, Vector2 tagetPosition, float delta);
        public delegate void MouseZoom(Mouse mouse, Vector2 tagetPosition, float distance, float delta);
        public event MouseDown onDown;
        public event MouseUp onUp;
        public event MouseStand onStand;
        public event MouseMove onMove;
        public event MouseScroll onScroll;
        public event MouseZoom onZoom;

        public float ZoomScale => zoomScale;
        public bool IsActive
        {
            set
            {
                if (!value)
                    Cancel();
                isActive = value;
            }
            get
            {
                return isActive;
            }
        }
        public bool IsDown => isDown;
        public bool IsMove => isMove;
        public float DownTime => downTime;
        public Vector2 PositionDown => positionDown;
        public Vector2 Position => position;
        #endregion Properties

        #region Method
        protected Mouse(LayerMask layerIgnore, float zoomScale)
        {
            this.layerIgnore = layerIgnore;
            this.zoomScale = zoomScale;
            //
            eventSystem = GetEventSystem();
        }
        public void Update()
        {
            if (!IsActive)
                return;
            OnUpdate();
        }
        public void Cancel()
        {
            OnCancel();
        }

        protected virtual void OnUpdate()
        {

        }
        protected virtual void OnCancel()
        {
            isDown = false;
            isMove = false;
            downTime = 0;
            positionDown = Vector2.zero;
            position = Vector2.zero;
        }
        protected void OnDown()
        {
            onDown?.Invoke(this);
        }
        protected void OnUp()
        {
            onUp?.Invoke(this);
        }
        protected void OnStand()
        {
            onStand?.Invoke(this);
        }
        protected void OnMove(Vector2 delta)
        {
            onMove?.Invoke(this, delta);
        }
        protected void OnScroll(Vector2 tagetPosition, float delta)
        {
            onScroll?.Invoke(this, tagetPosition, delta * zoomScale);
        }
        protected void OnZoom(Vector2 tagetPosition, float distance, float delta)
        {
            onZoom?.Invoke(this, tagetPosition, distance, delta * zoomScale);
        }

        protected bool IsPointerIgnore(Vector2 mousePosition)
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.position = mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(eventData, results);
            foreach (RaycastResult result in results)
            {
                if (layerIgnore == (layerIgnore | (1 << result.gameObject.layer)))
                    return true;
            }
            return false;
        }
        #endregion Method

        #region Static Method
        public static EventSystem GetEventSystem()
        {
            EventSystem currentEventSystem = EventSystem.current;
            if (currentEventSystem != null)
                return currentEventSystem;
            GameObject objectEventSystem = new GameObject(NAME_GAMEOBJECT_EVENTSYSTEM);
            currentEventSystem = objectEventSystem.AddComponent<EventSystem>();
            return currentEventSystem;
        }
        #endregion Static Method
    }
}
