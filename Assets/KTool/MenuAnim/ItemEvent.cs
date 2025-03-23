using DG.Tweening;
using UnityEngine;

namespace KTool.MenuAnim
{
    public class ItemEvent : Item
    {
        #region Properties
        [SerializeField]
        private float delay;

        public override ItemType Type => ItemType.Event;
        #endregion Properties

        #region Method
        protected override void Init_Child()
        {

        }
        public override void SetObjectActive(bool isActive)
        {

        }
        protected override Tween CreateAnim(UpdateType updateType, bool unscaleTime)
        {
            Tween tween = DOVirtual.DelayedCall(delay, OnAnim_Action)
                .SetUpdate(unscaleTime)
                .SetUpdate(updateType)
                .OnStart(OnAnim_Start)
                .OnComplete(OnAnim_End);
            return tween;
        }
        private void OnAnim_Action()
        {

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
