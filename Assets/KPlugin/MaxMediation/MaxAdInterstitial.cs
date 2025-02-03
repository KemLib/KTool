using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Ad;
using KTool.Init;

namespace KPlugin.MaxMediation
{
    public class MaxAdInterstitial : MaxAd, IAdInterstitial
    {
        #region Properties
        [SerializeField]
        [SelectAdId(MaxAdType.Interstitial)]
        private int indexAd = 0;
        [SerializeField]
        private bool isAutoReload = true;
        [SerializeField]
        private float maxSleepTime = 0;

        private bool isCreated,
            isLoading,
            isLoaded,
            isShow;
        private float sleepTime;
        private int attemptLoad;

        private IAd.OnClick onClick;
        private IAd.OnShowComplete onShowComplete;

        public override AdType AdType => AdType.Interstitial;
        public string AdId
        {
            get
            {
                MaxSettingId settingAdId = MaxSetting.Instance.Ad_Get(MaxAdType.Interstitial, indexAd);
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
        public bool IsReady => MaxSdk.IsInterstitialReady(AdId);
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
            AdManager.Instance.Add(this);
            //
            Create();
            Load();
        }

        protected override void OnInitEnd()
        {

        }
        #endregion

        #region Method
        public override void Create()
        {
            if (IsCreated)
                return;
            isCreated = true;
            Interstitial_EventRegister();
            //
            PushEvent_OnAdCreated(MaxAdType.Interstitial, true);
        }
        public override void Destroy()
        {
            if (!IsCreated)
                return;
            isCreated = false;
            Interstitial_EventUnRegister();
            //
            PushEvent_OnAdDestroy(MaxAdType.Interstitial);
        }
        public override void Load()
        {
            if (!IsCreated || IsLoading || IsShow)
                return;
            Interstitial_Loading();
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
            MaxSdk.ShowInterstitial(AdId);
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
            MaxSdk.ShowInterstitial(AdId);
        }
        private void Update_SleepTime()
        {
            if (sleepTime <= 0)
                return;
            sleepTime = Mathf.Max(0, sleepTime - Time.unscaledDeltaTime);
        }
        #endregion

        #region Max Interstitial
        private void Interstitial_EventRegister()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += Interstitial_OnAdLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += Interstitial_OnAdLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += Interstitial_OnAdDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += Interstitial_OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += Interstitial_OnAdClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += Interstitial_OnAdHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += Interstitial_OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Interstitial.OnAdReviewCreativeIdGeneratedEvent += Interstitial_OnAdReviewCreativeIdGeneratedEvent;
        }
        private void Interstitial_EventUnRegister()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= Interstitial_OnAdLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= Interstitial_OnAdLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= Interstitial_OnAdDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= Interstitial_OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent -= Interstitial_OnAdClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= Interstitial_OnAdHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= Interstitial_OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Interstitial.OnAdReviewCreativeIdGeneratedEvent -= Interstitial_OnAdReviewCreativeIdGeneratedEvent;
        }

        private void Interstitial_Loading()
        {
            isLoaded = false;
            isLoading = true;
            StartCoroutine(Interstitial_IE_Loading(0));
        }
        private IEnumerator Interstitial_IE_Loading(float delay)
        {
            while (MaxManager.Instance == null || !MaxManager.Instance.InitComplete || delay > 0)
            {
                delay = Mathf.Max(0, delay -= Time.unscaledDeltaTime);
                yield return new WaitForEndOfFrame();
            }
            //
            MaxSdk.LoadInterstitial(AdId);
        }

        private void Interstitial_OnAdLoadedEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdLoaded(MaxAdType.Interstitial, true);
        }
        private void Interstitial_OnAdLoadFailedEvent(string adId, MaxSdkBase.ErrorInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            attemptLoad = Mathf.Min(attemptLoad + 1, 6);
            float delay = Mathf.Pow(2, attemptLoad);
            StartCoroutine(Interstitial_IE_Loading(delay));
            //
            PushEvent_OnAdLoaded(MaxAdType.Interstitial, false);
        }
        private void Interstitial_OnAdDisplayedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            PushEvent_OnAdDisplayed(MaxAdType.Interstitial, true);
        }
        private void Interstitial_OnAdDisplayFailedEvent(string adId, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdDisplayed(MaxAdType.Interstitial, false);
            //
            if (IsAutoReload)
                Interstitial_Loading();
        }
        private void Interstitial_OnAdClickedEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdClicked(MaxAdType.Interstitial);
        }
        private void Interstitial_OnAdHiddenEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdHidden(MaxAdType.Interstitial);
            //
            if (IsAutoReload)
                Interstitial_Loading();
        }
        private void Interstitial_OnAdRevenuePaidEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdRevenuePaid(MaxAdType.Interstitial, revenuePaid);
        }
        private void Interstitial_OnAdReviewCreativeIdGeneratedEvent(string adId, string adReviewCreativeId, MaxSdkBase.AdInfo adInfo)
        {

        }
        #endregion
    }
}
