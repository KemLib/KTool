using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class Ad : MonoBehaviour
    {
        #region Properties
        public delegate void AdInitedDelegate(Ad source, bool isSuccess);
        public delegate void AdLoadedDelegate(Ad source, bool isSuccess);
        public delegate void AdDisplayedDelegate(Ad source, bool isSuccess);
        public delegate void AdHiddenDelegate(Ad source);
        public delegate void AdDestroyDelegate(Ad source);
        public delegate void AdClickedDelegate(Ad source);
        public delegate void AdRevenuePaidDelegate(Ad source, AdRevenuePaid revenuePaid);

        [SerializeField]
        private string adName;
        [SerializeField]
        private bool isAutoReload;

        private AdState state;
        private bool isShow;

        public event AdInitedDelegate OnAdInited;
        public event AdLoadedDelegate OnAdLoaded;
        public event AdDisplayedDelegate OnAdDisplayed;
        public event AdHiddenDelegate OnAdHidden;
        public event AdDestroyDelegate OnAdDestroy;
        public event AdClickedDelegate OnAdClicked;
        public event AdRevenuePaidDelegate OnAdRevenuePaid;

        public virtual string Name => string.IsNullOrEmpty(adName) ? gameObject.name : adName;
        public abstract AdType AdType
        {
            get;
        }
        public virtual bool IsInited
        {
            get => state == AdState.Inited || state == AdState.Loaded;
            protected set
            {
                if (state == AdState.None && value)
                    state = AdState.Inited;
            }
        }
        public virtual bool IsLoaded
        {
            get => state == AdState.Loaded;
            protected set
            {
                if (IsInited)
                    state = value ? AdState.Loaded : AdState.Inited;
            }
        }
        public virtual bool IsDestroy
        {
            get => state == AdState.Destroy;
            protected set
            {
                if (value)
                    state = AdState.Destroy;
            }
        }
        public virtual bool IsReady => IsLoaded && !isShow;
        public virtual bool IsShow
        {
            get => isShow;
            protected set => isShow = value;
        }
        public virtual bool IsAutoReload
        {
            get => isAutoReload;
            protected set => isAutoReload = value;
        }
        #endregion

        #region Methods
        public abstract void Init();
        public abstract void Load();
        public abstract void Destroy();
        #endregion

        #region Event
        protected void PushEvent_Inited(bool isSuccess)
        {
            OnAdInited?.Invoke(this, isSuccess);
        }
        protected void PushEvent_Loaded(bool isSuccess)
        {
            OnAdLoaded?.Invoke(this, isSuccess);
        }
        protected void PushEvent_Displayed(bool isSuccess)
        {
            OnAdDisplayed?.Invoke(this, isSuccess);
        }
        protected void PushEvent_Hidden()
        {
            OnAdHidden?.Invoke(this);
        }
        protected void PushEvent_Destroy()
        {
            OnAdDestroy?.Invoke(this);
        }
        protected void PushEvent_Clicked()
        {
            OnAdClicked?.Invoke(this);
        }
        protected void PushEvent_RevenuePaid(AdRevenuePaid revenuePaid)
        {
            OnAdRevenuePaid?.Invoke(this, revenuePaid);
        }
        #endregion
    }
}
