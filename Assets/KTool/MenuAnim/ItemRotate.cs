using DG.Tweening;
using UnityEngine;

namespace KTool.MenuAnim
{
    public class ItemRotate : Item
    {
        #region Properties
        [SerializeField]
        private RectTransform rtfObject;
        [SerializeField]
        private RotateMode rotateMode;
        [SerializeField]
        private bool useOrigin;
        [SerializeField]
        private float origin,
            taget;
        [SerializeField]
        private float delay;
        [SerializeField]
        private float duration;
        [SerializeField]
        private Ease doEase;

        public override ItemType Type => ItemType.Rotate;
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
            Vector3 currentAngle = rtfObject.rotation.eulerAngles;
            if (useOrigin)
                rtfObject.rotation = Quaternion.Euler(currentAngle.x, currentAngle.y, origin);
            Vector3 tagetAngle = new Vector3(currentAngle.x, currentAngle.y, taget);
            Tween tween = rtfObject.DOLocalRotate(tagetAngle, duration, rotateMode)
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
