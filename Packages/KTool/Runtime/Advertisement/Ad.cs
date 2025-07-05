using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class Ad : MonoBehaviour
    {
        #region Properties
        internal const string ERROR_AD_EVENT_INIT_EXCEPTION = "Ad {0} call event Inited exception: {1}",
            ERROR_AD_EVENT_LOADED_EXCEPTION = "Ad {0} call event Loaded exception: {1}",
            ERROR_AD_EVENT_DISPLAYED_EXCEPTION = "Ad {0} call event Displayed exception: {1}",
            ERROR_AD_EVENT_HIDDEN_EXCEPTION = "Ad {0} call event Hidden exception: {1}",
            ERROR_AD_EVENT_DESTROY_EXCEPTION = "Ad {0} call event Destroy exception: {1}",
            ERROR_AD_EVENT_CLICKED_EXCEPTION = "Ad {0} call event Clicked exception: {1}",
            ERROR_AD_EVENT_REVENUE_PAID_EXCEPTION = "Ad {0} call event RevenuePaid exception: {1}";

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
            try
            {
                OnAdInited?.Invoke(this, isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_INIT_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        protected void PushEvent_Loaded(bool isSuccess)
        {
            try
            {
                OnAdLoaded?.Invoke(this, isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_LOADED_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        protected void PushEvent_Displayed(bool isSuccess)
        {
            try
            {
                OnAdDisplayed?.Invoke(this, isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_DISPLAYED_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        protected void PushEvent_Hidden()
        {
            try
            {
                OnAdHidden?.Invoke(this);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_HIDDEN_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        protected void PushEvent_Destroy()
        {
            try
            {
                OnAdDestroy?.Invoke(this);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_DESTROY_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        protected void PushEvent_Clicked()
        {
            try
            {
                OnAdClicked?.Invoke(this);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_CLICKED_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        protected void PushEvent_RevenuePaid(AdRevenuePaid revenuePaid)
        {
            try
            {
                OnAdRevenuePaid?.Invoke(this, revenuePaid);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_REVENUE_PAID_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        #endregion
    }
}
