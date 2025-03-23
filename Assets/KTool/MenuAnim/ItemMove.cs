using DG.Tweening;
using UnityEngine;

namespace KTool.MenuAnim
{
    public class ItemMove : Item
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

        public override ItemType Type => ItemType.Move;
        #endregion Properties

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
                        rtfObject.anchoredPosition = origin;
                    tween = rtfObject.DOAnchorPos(taget, duration);
                    break;
                case RtfType.X:
                    if (useOrigin)
                        rtfObject.anchoredPosition = new Vector2(origin.x, rtfObject.anchoredPosition.y);
                    tween = rtfObject.DOAnchorPosX(taget.x, duration);
                    break;
                case RtfType.Y:
                    if (useOrigin)
                        rtfObject.anchoredPosition = new Vector2(rtfObject.anchoredPosition.x, origin.y);
                    tween = rtfObject.DOAnchorPosY(taget.y, duration);
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
        private void OnAnim_Start()
        {
            OnStart();
        }
        private void OnAnim_End()
        {
            OnComplete();
        }
        #endregion Method
    }
}
