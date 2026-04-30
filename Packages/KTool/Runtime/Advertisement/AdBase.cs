using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdBase : MonoBehaviour
    {
        #region Properties
        public delegate void AdLoadedDelegate(AdBase source, bool isSuccess);
        public delegate void AdDisplayedDelegate(AdBase source, bool isSuccess, string placement);
        public delegate void AdHiddenDelegate(AdBase source, string placement);
        public delegate void AdClickedDelegate(AdBase source, string placement);
        public delegate void AdRevenuePaidDelegate(AdBase source, AdRevenuePaid revenuePaid, string placement);
        public delegate void AdDestroyDelegate(AdBase source);

        [SerializeField]
        private string adName;
        [SerializeField]
        private bool isAutoReload;

        private AdState state;
        private bool isShow;

        public event AdLoadedDelegate OnAdLoaded;
        public event AdDisplayedDelegate OnAdDisplayed;
        public event AdHiddenDelegate OnAdHidden;
        public event AdClickedDelegate OnAdClicked;
        public event AdRevenuePaidDelegate OnAdRevenuePaid;
        public event AdDestroyDelegate OnAdDestroy;

        public virtual string Name => string.IsNullOrEmpty(adName) ? gameObject.name : adName;
        public abstract AdType AdType
        {
            get;
        }
        public virtual bool IsAutoReload
        {
            get => isAutoReload;
            protected set => isAutoReload = value;
        }
        public virtual bool IsLoaded
        {
            get => state == AdState.Loaded;
            protected set
            {
                if (IsDestroy)
                    return;
                state = value ? AdState.Loaded : AdState.None;
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
        #endregion

        #region Methods
        public abstract void Load();
        public abstract void Destroy();
        #endregion

        #region Event
        protected void PushEvent_Loaded(bool isSuccess)
        {
            OnAdLoaded?.Invoke(this, isSuccess);
        }
        protected internal void PushEvent_Displayed(bool isSuccess, string placement)
        {
            OnAdDisplayed?.Invoke(this, isSuccess, placement);
        }
        protected internal void PushEvent_Hidden(string placement)
        {
            OnAdHidden?.Invoke(this, placement);
        }
        protected internal void PushEvent_Clicked(string placement)
        {
            OnAdClicked?.Invoke(this, placement);
        }
        protected internal void PushEvent_RevenuePaid(AdRevenuePaid revenuePaid, string placement)
        {
            OnAdRevenuePaid?.Invoke(this, revenuePaid, placement);
        }
        protected void PushEvent_Destroy()
        {
            OnAdDestroy?.Invoke(this);
        }
        #endregion
    }
}
