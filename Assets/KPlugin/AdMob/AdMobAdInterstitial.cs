using GoogleMobileAds.Api;
using KTool.Ad;
using System;
using System.Collections;
using UnityEngine;

namespace KPlugin.AdMob
{
    public class AdMobAdInterstitial : AdMobAd, IAdInterstitial
    {
        #region Properties
        private const int AD_EXPIRE_HOUR = 1;

        [SerializeField]
        [SelectAdId(AdMobAdType.Interstitial)]
        private int indexAd = 0;
        [SerializeField]
        private bool isAutoReload = true;
        [SerializeField]
        private float maxSleepTime = 0;

        private bool isLoading,
            isShow;
        private float sleepTime;
        private int attemptLoad;
        private InterstitialAd adObject;
        private DateTime expireTime;
        private Coroutine coroutineAdCreate;

        private IAd.OnClick onClick;
        private IAd.OnShowComplete onShowComplete;

        public override AdType AdType => AdType.Interstitial;
        public string AdId
        {
            get
            {
                AdMobSettingAdId settingAdId = AdMobSetting.Instance.Ad_Get(AdMobAdType.Interstitial, indexAd);
                if (settingAdId != null)
                    return settingAdId.AdID;
                return string.Empty;
            }
        }
        public float MaxSleepTime
        {
            get => maxSleepTime;
            set
            {
                maxSleepTime = Mathf.Max(0, value);
                sleepTime = Mathf.Min(sleepTime, maxSleepTime);
            }
        }
        public override bool IsAutoReload
        {
            get => isAutoReload;
            set
            {
                if (value == isAutoReload)
                    return;
                isAutoReload = value;
                if (isAutoReload && !IsLoaded)
                    Load();
            }
        }
        public override bool IsCreated => (adObject != null);
        public override bool IsLoading => isLoading;
        public override bool IsLoaded => (adObject != null);
        public override bool IsShow => isShow;
        public bool IsReady => (IsCreated && IsLoaded && adObject.CanShowAd());
        #endregion

        #region Unity Event
        private void Update()
        {
            Update_SleepTime();
            Update_ExpireTime();
        }
        #endregion

        #region Init
        protected override void OnInitBegin()
        {
            AdManager.Instance.Add(this);
            //
            Create();
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
            Ad_Create();
        }
        public override void Destroy()
        {
            if (!IsCreated || IsShow)
                return;
            //
            Ad_Destroy();
        }
        public override void Load()
        {
            if (IsLoading || IsShow)
                return;
            Ad_Create();
        }
        public void Show(IAd.OnShowComplete onShowComplete = null)
        {
            if (!IsReady || IsShow || sleepTime > 0)
            {
                try
                {
                    onShowComplete?.Invoke(false);
                }
                catch (Exception ex)
                {
                    string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_SHOW_COMPLETE, ex.Message);
                    Debug.LogError(message);
                }
                return;
            }
            onClick = null;
            this.onShowComplete = onShowComplete;
            isShow = true;
            sleepTime = maxSleepTime;
            adObject.Show();
        }
        public void Show(IAd.OnClick onClick, IAd.OnShowComplete onShowComplete = null)
        {
            if (!IsReady || IsShow || sleepTime > 0)
            {
                try
                {
                    onShowComplete?.Invoke(false);
                }
                catch (Exception ex)
                {
                    string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_SHOW_COMPLETE, ex.Message);
                    Debug.LogError(message);
                }
                return;
            }
            this.onClick = onClick;
            this.onShowComplete = onShowComplete;
            isShow = true;
            sleepTime = maxSleepTime;
            adObject.Show();
        }
        private void Update_SleepTime()
        {
            if (sleepTime <= 0)
                return;
            sleepTime = Mathf.Max(0, sleepTime - Time.unscaledDeltaTime);
        }
        private void Update_ExpireTime()
        {
            if (!IsLoaded || IsShow)
                return;
            if (DateTime.Now < expireTime)
                return;
            Ad_Create();
        }
        #endregion

        #region Ad
        private void Ad_Create()
        {
            Ad_Destroy();
            isLoading = true;
            if (coroutineAdCreate != null)
                StopCoroutine(coroutineAdCreate);
            coroutineAdCreate = StartCoroutine(Ad_IE_Create(0));
        }
        private void Ad_Destroy()
        {
            if (adObject == null)
                return;
            //
            adObject.Destroy();
            adObject = null;
            PushEvent_OnAdDestroy(AdMobAdType.AppOpen);
        }
        private IEnumerator Ad_IE_Create(float delay)
        {
            while (AdMobManager.Instance == null || !AdMobManager.Instance.InitComplete || delay > 0)
            {
                delay = Mathf.Max(0, delay - Time.unscaledDeltaTime);
                yield return new WaitForEndOfFrame();
            }
            //
            coroutineAdCreate = null;
            Ad_LoadAd();
        }
        private void Ad_LoadAd()
        {
            AdRequest adRequest = new AdRequest();
            InterstitialAd.Load(AdId, adRequest, Ad_OnLoadComplete);
        }
        private void Ad_OnLoadComplete(InterstitialAd adObject, LoadAdError error)
        {
            if (error != null || adObject == null)
            {
                attemptLoad = Mathf.Min(attemptLoad + 1, 6);
                float delay = Mathf.Pow(2, attemptLoad);
                if (coroutineAdCreate != null)
                    StopCoroutine(coroutineAdCreate);
                coroutineAdCreate = StartCoroutine(Ad_IE_Create(delay));
                //
                PushEvent_OnAdCreated(AdMobAdType.Interstitial, false);
                return;
            }
            //
            attemptLoad = 0;
            isLoading = false;
            this.adObject = adObject;
            expireTime = DateTime.Now + TimeSpan.FromHours(AD_EXPIRE_HOUR);
            if (!InitComplete)
                InitComplete = true;
            Ad_EventRegister();
            //
            PushEvent_OnAdCreated(AdMobAdType.Interstitial, true);
            PushEvent_OnAdLoaded(AdMobAdType.Interstitial, true);
        }
        private void Ad_EventRegister()
        {
            adObject.OnAdFullScreenContentOpened += Ad_OnAdFullScreenContentOpened;
            adObject.OnAdFullScreenContentFailed += Ad_OnAdFullScreenContentFailed;
            adObject.OnAdClicked += Ad_OnAdClicked;
            adObject.OnAdPaid += Ad_OnAdPaid;
            adObject.OnAdFullScreenContentClosed += Ad_OnAdFullScreenContentClosed;
            adObject.OnAdImpressionRecorded += Ad_OnAdImpressionRecorded;
        }
        private void Ad_OnAdFullScreenContentOpened()
        {
            PushEvent_OnAdDisplayed(AdMobAdType.Interstitial, true);
        }
        private void Ad_OnAdFullScreenContentFailed(AdError adError)
        {
            isShow = false;
            //
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
            PushEvent_OnAdDisplayed(AdMobAdType.Interstitial, false);
            //
            Ad_Destroy();
            if (IsAutoReload)
                Ad_Create();
        }
        private void Ad_OnAdClicked()
        {
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
            PushEvent_OnAdClicked(AdMobAdType.Interstitial);
        }
        private void Ad_OnAdFullScreenContentClosed()
        {
            isShow = false;
            //
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
            PushEvent_OnAdHidden(AdMobAdType.Interstitial);
            //
            Ad_Destroy();
            if (IsAutoReload)
                Ad_Create();
        }
        private void Ad_OnAdPaid(AdValue adValue)
        {
            AdRevenuePaid revenuePaid = new AdRevenuePaid(
                AdMobManager.ADMOB_SCOURCE,
                AdMobManager.ADMOB_SCOURCE,
                AdId,
                AdMobManager.ADMOB_COUNTRY_CODE,
                AdType,
                adValue.Value,
                adValue.CurrencyCode);
            PushEvent_OnAdRevenuePaid(AdMobAdType.Interstitial, revenuePaid);
        }
        private void Ad_OnAdImpressionRecorded()
        {

        }
        #endregion
    }
}
