using DG.Tweening;
using UnityEngine;

namespace KTool.MenuAnim
{
    public class ItemActive : Item
    {
        #region Properties
        [SerializeField]
        private GameObject gameObject;
        [SerializeField]
        private float delay;
        [SerializeField]
        private bool isActive;

        public override ItemType Type => ItemType.Active;
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
            gameObject.SetActive(isActive);
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
