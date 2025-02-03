using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.MenuAnim
{
    public class ItemColor : Item
    {
        #region Properties
        [SerializeField]
        private MaskableGraphic graphic;
        [SerializeField]
        private bool useOrigin;
        [SerializeField]
        private Color origin,
            taget;
        [SerializeField]
        private float delay;
        [SerializeField]
        private float duration;
        [SerializeField]
        private Ease doEase;

        public override ItemType Type => ItemType.Color;
        #endregion Properties

        #region Method
        protected override void Init_Child()
        {

        }
        public override void SetObjectActive(bool isActive)
        {
            if (graphic == null)
                return;
            graphic.gameObject.SetActive(isActive);
        }
        protected override Tween CreateAnim(UpdateType updateType, bool unscaleTime)
        {
            if (graphic == null)
                return null;
            if (useOrigin)
                graphic.color = origin;
            Tween tween = graphic.DOColor(taget, duration)
                .SetUpdate(unscaleTime)
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
