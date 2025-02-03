using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Ad;
using KTool.Init;

namespace KPlugin.MaxMediation
{
    public class MaxAdAppOpen : MaxAd, IAdAppOpen
    {
        #region Properties
        [SerializeField]
        [SelectAdId(MaxAdType.AppOpen)]
        private int indexAd = 0;
        [SerializeField]
        private bool isAutoReload = true;
        [SerializeField]
        private float maxSleepTime = 0;
        [SerializeField]
        private bool isShowAfterInit = true;

        private bool isCreated,
            isLoading,
            isLoaded,
            isShow;
        private float sleepTime;
        private int attemptLoad;

        private IAd.OnClick onClick;
        private IAd.OnShowComplete onShowComplete;

        public override AdType AdType => AdType.AppOpen;
        public string AdId
        {
            get
            {
                MaxSettingId settingAdId = MaxSetting.Instance.Ad_Get(MaxAdType.AppOpen, indexAd);
                if (settingAdId != null)
                    return settingAdId.AdID;
                return string.Empty;
            }
        }
        public float MaxSleepTime
        {
            get => maxSleepTime;
            set => Mathf.Max(0, value);
        }
        public override bool IsAutoReload
        {
            get => isAutoReload;
            set
            {
                if (value == isAutoReload)
                    return;
                isAutoReload = value;
                if (isAutoReload)
                    Load();
            }
        }
        public override bool IsCreated => isCreated;
        public override bool IsLoading => isLoading;
        public override bool IsLoaded => isLoaded;
        public override bool IsShow => isShow;
        public bool IsReady => MaxSdk.IsAppOpenAdReady(AdId);
        #endregion

        #region Unity Event
        private void Update()
        {
            Update_SleepTime();
        }
        #endregion

        #region Init
        protected override void OnInitBegin()
        {
            IAdAppOpen.Instance = this;
            //
            Create();
            Load();
        }

        protected override void OnInitEnd()
        {
            if (isShowAfterInit)
                Show();
        }
        #endregion

        #region Method
        public override void Create()
        {
            if (IsCreated)
                return;
            isCreated = true;
            AppOpen_EventRegister();
            //
            PushEvent_OnAdCreated(MaxAdType.AppOpen, true);
        }

        public override void Destroy()
        {
            if (!IsCreated)
                return;
            isCreated = false;
            AppOpen_EventUnRegister();
            //
            PushEvent_OnAdDestroy(MaxAdType.AppOpen);
        }

        public override void Load()
        {
            if (!IsCreated || IsLoading || IsShow)
                return;
            AppOpen_Loading();
        }

        public void Show(IAd.OnShowComplete onShowComplete = null)
        {
            if (!IsCreated || !IsLoaded || IsLoading || !IsReady || IsShow || sleepTime > 0)
            {
                onShowComplete?.Invoke(false);
                return;
            }
            onClick = null;
            this.onShowComplete = onShowComplete;
            isShow = true;
            sleepTime = maxSleepTime;
            MaxSdk.ShowAppOpenAd(AdId);
        }

        public void Show(IAd.OnClick onClick, IAd.OnShowComplete onShowComplete = null)
        {
            if (!IsCreated || !IsLoaded || IsLoading || !IsReady || IsShow || sleepTime > 0)
            {
                onShowComplete?.Invoke(false);
                return;
            }
            this.onClick = onClick;
            this.onShowComplete = onShowComplete;
            isShow = true;
            sleepTime = maxSleepTime;
            MaxSdk.ShowAppOpenAd(AdId);
        }
        private void Update_SleepTime()
        {
            if (sleepTime <= 0)
                return;
            sleepTime = Mathf.Max(0, sleepTime - Time.unscaledDeltaTime);
        }
        #endregion

        #region Max AppOpen
        private void AppOpen_EventRegister()
        {
            MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += AppOpen_OnAdLoadedEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += AppOpen_OnAdLoadFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += AppOpen_OnAdDisplayedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += AppOpen_OnAdDisplayFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdClickedEvent += AppOpen_OnAdClickedEvent;
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += AppOpen_OnAdHiddenEvent;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += AppOpen_OnAdRevenuePaidEvent;
        }
        private void AppOpen_EventUnRegister()
        {
            MaxSdkCallbacks.AppOpen.OnAdLoadedEvent -= AppOpen_OnAdLoadedEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent -= AppOpen_OnAdLoadFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent -= AppOpen_OnAdDisplayedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent -= AppOpen_OnAdDisplayFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdClickedEvent -= AppOpen_OnAdClickedEvent;
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent -= AppOpen_OnAdHiddenEvent;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent -= AppOpen_OnAdRevenuePaidEvent;
        }

        private void AppOpen_Loading()
        {
            isLoaded = false;
            isLoading = true;
            StartCoroutine(AppOpen_IE_Loading(0));
        }
        private IEnumerator AppOpen_IE_Loading(float delay)
        {
            while (MaxManager.Instance == null || !MaxManager.Instance.InitComplete || delay > 0)
            {
                delay = Mathf.Max(0, delay -= Time.unscaledDeltaTime);
                yield return new WaitForEndOfFrame();
            }
            //
            MaxSdk.LoadAppOpenAd(AdId);
        }

        private void AppOpen_OnAdLoadedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            isLoaded = true;
            isLoading = false;
            attemptLoad = 0;
            if (!InitComplete)
                InitComplete = true;
            //
            PushEvent_OnAdLoaded(MaxAdType.AppOpen, true);
        }
        private void AppOpen_OnAdLoadFailedEvent(string adId, MaxSdkBase.ErrorInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            attemptLoad = Mathf.Min(attemptLoad + 1, 6);
            float delay = Mathf.Pow(2, attemptLoad);
            StartCoroutine(AppOpen_IE_Loading(delay));
            //
            PushEvent_OnAdLoaded(MaxAdType.AppOpen, false);
        }
        private void AppOpen_OnAdDisplayedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            PushEvent_OnAdDisplayed(MaxAdType.AppOpen, true);
        }
        private void AppOpen_OnAdDisplayFailedEvent(string adId, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            isShow = false;
            isLoaded = false;
            try
            {
                onShowComplete?.Invoke(false);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_SHOW_COMPLETE, ex.Message);
                Debug.LogError(message);
            }
            finally
            {
                onShowComplete = null;
            }
            //
            PushEvent_OnAdDisplayed(MaxAdType.AppOpen, false);
            //
            if (IsAutoReload)
                AppOpen_Loading();
        }
        private void AppOpen_OnAdClickedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            try
            {
                onClick?.Invoke();
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_CLICK, ex.Message);
                Debug.LogError(message);
            }
            finally
            {
                onClick = null;
            }
            //
            PushEvent_OnAdClicked(MaxAdType.AppOpen);
        }
        private void AppOpen_OnAdHiddenEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            isShow = false;
            isLoaded = false;
            try
            {
                onShowComplete?.Invoke(true);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_SHOW_COMPLETE, ex.Message);
                Debug.LogError(message);
            }
            finally
            {
                onShowComplete = null;
            }
            //
            PushEvent_OnAdHidden(MaxAdType.AppOpen);
            //
            if (IsAutoReload)
                AppOpen_Loading();
        }
        private void AppOpen_OnAdRevenuePaidEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            AdRevenuePaid revenuePaid = new AdRevenuePaid(
                MaxManager.MAX_SCOURCE,
                adInfo.NetworkName,
                AdId,
                MaxManager.Instance.CountryCode,
                AdType,
                adInfo.Revenue,
                MaxManager.MAX_CURRENCY);
            PushEvent_OnAdRevenuePaid(MaxAdType.AppOpen, revenuePaid);
        }
        #endregion
    }
}
