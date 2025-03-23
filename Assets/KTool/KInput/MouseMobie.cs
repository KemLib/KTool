using UnityEngine;

namespace KTool.KInput
{
    public class MouseMobie : Mouse
    {
        #region Properties
        private int touchIdMain,
            touchIdAlpha,
            touchIdBeta;
        private bool isTouchAlpha,
            isTouchBeta;
        private float touchDistance;
        #endregion Properties

        #region Method
        public MouseMobie(LayerMask layerIgnore, float zoomScale) : base(layerIgnore, zoomScale)
        {
            touchDistance = -1;
            isTouchAlpha = false;
            isTouchBeta = false;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //
            if (Input.touchCount > 1)
            {
                isDown = false;
                isMove = false;
                //OnUp();
                //
                Update_Zoom();
            }
            else
            {
                if (IsDown)
                    Update_Down();
                else
                    Update_NotDown();
            }
        }
        protected override void OnCancel()
        {
            base.OnCancel();
            //
            isTouchAlpha = false;
            isTouchBeta = false;
            touchDistance = -1;
        }
        private void Update_Down()
        {
            downTime += Time.unscaledDeltaTime;
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.fingerId != touchIdMain)
                    continue;
                if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {
                    isDown = false;
                    isMove = false;
                    position = touch.position;
                    OnUp();
                    return;
                }

                if (touch.position == position)
                {
                    isMove = false;
                    position = touch.position;
                    OnStand();
                }
                else
                {
                    isMove = true;
                    Vector2 delta = touch.position - position;
                    position = touch.position;
                    OnMove(delta);
                }
                return;
            }
            //
            isDown = false;
            isMove = false;
            OnUp();
        }
        private void Update_NotDown()
        {
            if (Input.touchCount == 0)
                return;
            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began || IsPointerIgnore(touch.position))
                return;
            touchIdMain = touch.fingerId;
            isDown = true;
            downTime = 0;
            position = Input.mousePosition;
            positionDown = Input.mousePosition;
            OnDown();
        }

        private void Update_Zoom()
        {
            //
            if (!isTouchAlpha && !isTouchBeta)
            {
                Touch touchA = Input.GetTouch(0),
                    touchB = Input.GetTouch(1);
                isTouchAlpha = true;
                isTouchBeta = true;
                touchIdAlpha = touchA.fingerId;
                touchIdBeta = touchB.fingerId;
                touchDistance = -1;
                Update_Zoom(touchA, touchB);
                return;
            }
            //
            if (isTouchAlpha && isTouchBeta)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touchA = Input.GetTouch(i);
                    if (touchA.fingerId != touchIdAlpha)
                        continue;
                    for (int j = 0; j < Input.touchCount; j++)
                    {
                        Touch touchB = Input.GetTouch(j);
                        if (touchB.fingerId != touchIdBeta)
                            continue;
                        Update_Zoom(touchA, touchB);
                        return;
                    }
                    isTouchBeta = false;
                    touchDistance = -1;
                    return;
                }
                isTouchAlpha = false;
                touchDistance = -1;
                return;
            }
            //
            if (isTouchAlpha)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touchA = Input.GetTouch(i);
                    if (touchA.fingerId != touchIdAlpha)
                        continue;
                    for (int j = 0; j < Input.touchCount; j++)
                    {
                        Touch touchB = Input.GetTouch(j);
                        if (touchB.fingerId == touchIdAlpha)
                            continue;
                        isTouchBeta = true;
                        touchIdBeta = touchB.fingerId;
                        Update_Zoom(touchA, touchB);
                        return;
                    }
                    touchDistance = -1;
                    return;
                }
                isTouchAlpha = false;
                touchDistance = -1;
                return;
            }
            //
            if (isTouchBeta)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touchB = Input.GetTouch(i);
                    if (touchB.fingerId != touchIdBeta)
                        continue;
                    for (int j = 0; j < Input.touchCount; j++)
                    {
                        Touch touchA = Input.GetTouch(j);
                        if (touchA.fingerId == touchIdBeta)
                            continue;
                        isTouchAlpha = true;
                        touchIdAlpha = touchA.fingerId;
                        Update_Zoom(touchA, touchB);
                        return;
                    }
                    touchDistance = -1;
                    return;
                }
                isTouchBeta = false;
                touchDistance = -1;
                return;
            }
        }
        private void Update_Zoom(Touch touchA, Touch touchB)
        {
            if (touchA.phase == TouchPhase.Ended || touchA.phase == TouchPhase.Canceled)
            {
                isTouchAlpha = false;
                touchDistance = -1;
                return;
            }
            if (touchB.phase == TouchPhase.Ended || touchB.phase == TouchPhase.Canceled)
            {
                isTouchBeta = false;
                touchDistance = -1;
                return;
            }
            //
            if (touchDistance < 0)
            {
                touchDistance = Vector2.Distance(touchA.position, touchB.position);
                return;
            }
            float newTouchDistance = Vector2.Distance(touchA.position, touchB.position);
            if (newTouchDistance == touchDistance)
            {
                return;
            }
            Vector2 tagetPosition = (touchA.position + touchB.position) / 2;
            float delta = newTouchDistance - touchDistance;
            touchDistance = newTouchDistance;
            OnZoom(tagetPosition, touchDistance, delta);
        }
        #endregion Method
    }
}
