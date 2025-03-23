using UnityEngine;

namespace KTool.KInput
{
    public class MouseStandalone : Mouse
    {
        #region Properties

        #endregion Properties

        #region Method
        public MouseStandalone(LayerMask layerIgnore, float zoomScale) : base(layerIgnore, zoomScale)
        {

        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
            //
            if (IsDown)
                Update_Down();
            else
                Update_NotDown();
            Update_Scroll();
        }
        protected override void OnCancel()
        {
            base.OnCancel();
        }

        private void Update_Down()
        {
            downTime += Time.unscaledDeltaTime;
            Vector2 mousePosition = Input.mousePosition;
            if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
            {
                isDown = false;
                isMove = false;
                position = mousePosition;
                OnUp();
                return;
            }
            //
            if (mousePosition == position)
            {
                isMove = false;
                position = mousePosition;
                OnStand();
            }
            else
            {
                isMove = true;
                Vector2 delta = mousePosition - position;
                position = mousePosition;
                OnMove(delta);
            }
        }
        private void Update_NotDown()
        {
            if (!Input.GetMouseButtonDown(0))
                return;
            if (IsPointerIgnore(Input.mousePosition))
                return;
            isDown = true;
            downTime = 0;
            position = Input.mousePosition;
            positionDown = Input.mousePosition;
            OnDown();
        }
        private void Update_Scroll()
        {
            if (Input.mouseScrollDelta.y == 0)
                return;
            Vector2 tagetPosition = Input.mousePosition;
            float delta = -Input.mouseScrollDelta.y;
            OnScroll(tagetPosition, delta);
        }
        #endregion Method
    }
}
