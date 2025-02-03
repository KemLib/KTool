using GoogleMobileAds.Api;
using KTool.Ad;
using System;
using System.Collections;
using UnityEngine;

namespace KPlugin.AdMob
{
    public class AdMobAdRewardedInterstitial : AdMobAd, IAdRewardedInterstitial
    {
        #region Properties
        private const int AD_EXPIRE_HOUR = 1;

        [SerializeField]
        [SelectAdId(AdMobAdType.RewardedInterstitial)]
        private int indexAd = 0;
        [SerializeField]
        private bool isAutoReload = true;

        private bool isLoading,
            isShow;
        private int attemptLoad;
        private RewardedInterstitialAd adObject;
        private DateTime expireTime;
        private Coroutine coroutineAdCreate;

        public event IAd.OnAdReceivedReward OnAdReceivedRewardEvent;
        private IAd.OnClick onClick;
        private IAd.OnAdReceivedReward onReward;
        private IAd.OnShowComplete onShowComplete;

        public override AdType AdType => AdType.RewardedInterstitial;
        public string AdId
        {
            get
            {
                AdMobSettingAdId settingAdId = AdMobSetting.Instance.Ad_Get(AdMobAdType.RewardedInterstitial, indexAd);
                if (settingAdId != null)
                    return settingAdId.AdID;
                return string.Empty;
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
        public void Show(IAd.OnAdReceivedReward onReward, IAd.OnShowComplete onShowComplete = null)
        {
            if (!IsReady || IsShow)
            {
                try
                {
                    onReward?.Invoke(AdRewardReceived.RewardFail);
                }
                catch (Exception ex)
                {
                    string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_REWARD, ex.Message);
                    Debug.LogError(message);
                }
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
                return;
            }
            onClick = null;
            this.onReward = onReward;
            this.onShowComplete = onShowComplete;
            isShow = true;
            adObject.Show(Ad_OnReward);
        }
        public void Show(IAd.OnClick onClick, IAd.OnAdReceivedReward onReward, IAd.OnShowComplete onShowComplete = null)
        {
            if (!IsReady || IsShow)
            {
                try
                {
                    onReward?.Invoke(AdRewardReceived.RewardFail);
                }
                catch (Exception ex)
                {
                    string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_REWARD, ex.Message);
                    Debug.LogError(message);
                }
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
                return;
            }
            this.onClick = onClick;
            this.onReward = onReward;
            this.onShowComplete = onShowComplete;
            isShow = true;
            adObject.Show(Ad_OnReward);
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
            RewardedInterstitialAd.Load(AdId, adRequest, Ad_OnLoadComplete);
        }
        private void Ad_OnLoadComplete(RewardedInterstitialAd adObject, LoadAdError error)
        {
            if (error != null || adObject == null)
            {
                attemptLoad = Mathf.Min(attemptLoad + 1, 6);
                float delay = Mathf.Pow(2, attemptLoad);
                if (coroutineAdCreate != null)
                    StopCoroutine(coroutineAdCreate);
                coroutineAdCreate = StartCoroutine(Ad_IE_Create(delay));
                //
                PushEvent_OnAdCreated(AdMobAdType.RewardedInterstitial, false);
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
            PushEvent_OnAdCreated(AdMobAdType.RewardedInterstitial, true);
            PushEvent_OnAdLoaded(AdMobAdType.RewardedInterstitial, true);
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
            PushEvent_OnAdDisplayed(AdMobAdType.RewardedInterstitial, true);
        }
        private void Ad_OnAdFullScreenContentFailed(AdError adError)
        {
            isShow = false;
            //
            Ad_OnReward(AdRewardReceived.RewardFail);
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
            PushEvent_OnAdDisplayed(AdMobAdType.RewardedInterstitial, false);
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
            PushEvent_OnAdClicked(AdMobAdType.RewardedInterstitial);
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
            PushEvent_OnAdHidden(AdMobAdType.RewardedInterstitial);
            //
            Ad_Destroy();
            if (IsAutoReload)
                Ad_Create();
        }
        private void Ad_OnReward(Reward reward)
        {
            AdRewardReceived rewardReceived = new AdRewardReceived(reward.Type, reward.Amount);
            Ad_OnReward(rewardReceived);
        }
        private void Ad_OnReward(AdRewardReceived rewardReceived)
        {
            try
            {
                onReward?.Invoke(rewardReceived);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_REWARD, ex.Message);
                Debug.LogError(message);
            }
            finally
            {
                onReward = null;
            }
            //
            try
            {
                OnAdReceivedRewardEvent?.Invoke(rewardReceived);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, AdType.ToString(), EVENT_NAME_ON_AD_REWARD, ex.Message);
                Debug.LogError(message);
            }
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
            PushEvent_OnAdRevenuePaid(AdMobAdType.RewardedInterstitial, revenuePaid);
        }
        private void Ad_OnAdImpressionRecorded()
        {

        }
        #endregion
    }
}
