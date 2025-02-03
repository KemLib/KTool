using DG.Tweening;
using UnityEngine;

namespace KTool.MenuAnim
{
    public class ItemResize : Item
    {
        #region Properties
        [SerializeField]
        private RectTransform rtfObject;
        [SerializeField]
        private RtfType rtfType;
        [SerializeField]
        private bool useOrigin;
        [SerializeField]
        private Vector2 origin,
            taget;
        [SerializeField]
        private float delay;
        [SerializeField]
        private float duration;
        [SerializeField]
        private Ease doEase;

        public override ItemType Type => ItemType.Resize;
        #endregion

        #region Method
        protected override void Init_Child()
        {

        }
        public override void SetObjectActive(bool isActive)
        {
            if (rtfObject == null)
                return;
            rtfObject.gameObject.SetActive(isActive);
        }
        protected override Tween CreateAnim(UpdateType updateType, bool unscaleTime)
        {
            if (rtfObject == null)
                return null;
            Tween tween = null;
            switch (rtfType)
            {
                case RtfType.XY:
                    if (useOrigin)
                        rtfObject.sizeDelta = origin;
                    tween = rtfObject.DOSizeDelta(taget, duration);
                    break;
                case RtfType.X:
                    if (useOrigin)
                        rtfObject.anchoredPosition = new Vector2(origin.x, rtfObject.anchoredPosition.y);
                    tween = DOVirtual.Float(rtfObject.sizeDelta.x, taget.x, duration, OnAnim_ActionX);
                    break;
                case RtfType.Y:
                    if (useOrigin)
                        rtfObject.anchoredPosition = new Vector2(rtfObject.anchoredPosition.x, origin.y);
                    tween = DOVirtual.Float(rtfObject.sizeDelta.y, taget.y, duration, OnAnim_ActionY);
                    break;
                default:
                    return null;
            }
            tween.SetUpdate(unscaleTime)
                .SetUpdate(updateType)
                .SetDelay(delay)
                .SetEase(doEase)
                .OnStart(OnAnim_Start)
                .OnComplete(OnAnim_End);
            return tween;
        }
        private void OnAnim_ActionX(float sizeX)
        {
            rtfObject.sizeDelta = new Vector2(sizeX, rtfObject.sizeDelta.y);
        }
        private void OnAnim_ActionY(float sizeY)
        {
            rtfObject.sizeDelta = new Vector2(rtfObject.sizeDelta.x, sizeY);
        }
        private void OnAnim_Start()
        {
            OnStart();
        }
        private void OnAnim_End()
        {
            OnComplete();
        }
        #endregion
    }
}
