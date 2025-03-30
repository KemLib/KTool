using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class Ad : MonoBehaviour
    {
        #region Properties
        internal const string ERROR_AD_EVENT_INIT_EXCEPTION = "Ad {0} call event Init exception: {1}",
            ERROR_AD_EVENT_LOADED_EXCEPTION = "Ad {0} call event Loaded exception: {1}",
            ERROR_AD_EVENT_DISPLAYED_EXCEPTION = "Ad {0} call event Init exception: {1}",
            ERROR_AD_EVENT_SHOW_COMPLETE_EXCEPTION = "Ad {0} call event ShowComplete exception: {1}",
            ERROR_AD_EVENT_HIDDEN_EXCEPTION = "Ad {0} call event Hidden exception: {1}",
            ERROR_AD_EVENT_DESTROY_EXCEPTION = "Ad {0} call event Destroy exception: {1}",
            ERROR_AD_EVENT_CLICKED_EXCEPTION = "Ad {0} call event Clicked exception: {1}",
            ERROR_AD_EVENT_REVENUEPAID_EXCEPTION = "Ad {0} call event Init exception: {1}";

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
        [SerializeField]
        private bool isAutoReload;

        private AdState state;

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
        public virtual bool IsInited => State == AdState.Inited || State == AdState.Loaded || State == AdState.Ready || State == AdState.Show;
        public virtual bool IsLoaded => State == AdState.Loaded || State == AdState.Ready || State == AdState.Show;
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
            try
            {
                OnAdInited?.Invoke();
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
                OnAdLoaded?.Invoke(isSuccess);
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
                OnAdDisplayed?.Invoke(isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_DISPLAYED_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        protected void PushEvent_ShowComplete(bool isSuccess)
        {
            try
            {
                OnAdShowComplete?.Invoke(isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_SHOW_COMPLETE_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        protected void PushEvent_Hidden()
        {
            try
            {
                OnAdHidden?.Invoke();
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
                OnAdDestroy?.Invoke();
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
                OnAdClicked?.Invoke();
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
                OnAdRevenuePaid?.Invoke(revenuePaid);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_REVENUEPAID_EXCEPTION, AdType.ToString(), ex.Message));
            }
        }
        #endregion
    }
}
