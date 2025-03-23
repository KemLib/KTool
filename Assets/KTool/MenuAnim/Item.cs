using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.MenuAnim
{
    [System.Serializable]
    public abstract class Item
    {
        #region Properties
        [SerializeField]
        protected UnityEvent onStart,
            onEnd;

        private Anim anim;
        private Tween tween;
        private bool isPlay;

        public abstract ItemType Type
        {
            get;
        }
        public bool IsPlay => isPlay;
        #endregion Properties

        #region UnityEvent

        #endregion UnityEvent

        #region Method
        public void Init(Anim anim)
        {
            this.anim = anim;
            Init_Child();
        }
        public abstract void SetObjectActive(bool isActive);
        public void Start(UpdateType updateType, bool unscaleTime)
        {
            isPlay = true;
            tween = CreateAnim(updateType, unscaleTime);
        }
        public void Stop()
        {
            if (tween == null)
                return;
            tween.Kill();
            tween = null;
            isPlay = false;
        }
        public void ManualUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if (tween == null)
                return;
            tween.ManualUpdate(deltaTime, unscaleDeltaTime);
        }
        protected abstract void Init_Child();
        protected abstract Tween CreateAnim(UpdateType updateType, bool unscaleTime);
        protected void OnStart()
        {
            onStart?.Invoke();
        }
        protected void OnComplete()
        {
            onEnd?.Invoke();
            anim.OnComplete(this);
            tween = null;
            isPlay = false;
        }
        #endregion Method
    }
}
