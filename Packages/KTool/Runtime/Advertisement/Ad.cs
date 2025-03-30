using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class Ad : MonoBehaviour
    {
        #region Properties
        public delegate void AdInitDelegate();
        public delegate void AdLoadedDelegate(bool isSuccess);
        public delegate void AdDisplayedDelegate(bool isSuccess);
        public delegate void AdShowCompleteDelegate(bool isSuccess);
        public delegate void AdHiddenDelegate();
        public delegate void AdDestroyDelegate();
        public delegate void AdClickedDelegate();
        public delegate void AdRevenuePaidDelegate(AdRevenuePaid revenuePaid);

        [SerializeField]
        private string adName;

        private AdState state;
        private bool isAutoReload;

        public event AdInitDelegate OnAdInited;
        public event AdLoadedDelegate OnAdLoaded;
        public event AdDisplayedDelegate OnAdDisplayed;
        public event AdShowCompleteDelegate OnAdShowComplete;
        public event AdHiddenDelegate OnAdHidden;
        public event AdDestroyDelegate OnAdDestroy;
        public event AdClickedDelegate OnAdClicked;
        public event AdRevenuePaidDelegate OnAdRevenuePaid;

        public abstract AdType AdType
        {
            get;
        }
        public virtual string Name => string.IsNullOrEmpty(adName) ? gameObject.name : adName;
        public virtual AdState State
        {
            get => state;
            protected set => state = value;
        }
        public virtual bool IsAutoReload
        {
            get => isAutoReload;
            protected set => isAutoReload = value;
        }
        public virtual bool IsInited => State == (AdState.Inited | AdState.Loaded | AdState.Ready | AdState.Show);
        public virtual bool IsLoaded => State == (AdState.Loaded | AdState.Ready | AdState.Show);
        public virtual bool IsReady => State == AdState.Ready;
        public virtual bool IsShow => State == AdState.Show;
        public virtual bool IsDestroy => State == AdState.Destroy;
        #endregion

        #region Methods
        public abstract void Init();
        public abstract void Load();
        public abstract void Destroy();
        #endregion

        #region Event
        protected void PushEvent_Inited()
        {
            OnAdInited?.Invoke();
        }
        protected void PushEvent_Loaded(bool isSuccess)
        {
            OnAdLoaded?.Invoke(isSuccess);
        }
        protected void PushEvent_Displayed(bool isSuccess)
        {
            OnAdDisplayed?.Invoke(isSuccess);
        }
        protected void PushEvent_ShowComplete(bool isSuccess)
        {
            OnAdShowComplete?.Invoke(isSuccess);
        }
        protected void PushEvent_Hidden()
        {
            OnAdHidden?.Invoke();
        }
        protected void PushEvent_Destroy()
        {
            OnAdDestroy?.Invoke();
        }
        protected void PushEvent_Clicked()
        {
            OnAdClicked?.Invoke();
        }
        protected void PushEvent_RevenuePaid(AdRevenuePaid revenuePaid)
        {
            OnAdRevenuePaid?.Invoke(revenuePaid);
        }
        #endregion
    }
}
