using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Ad;
using KTool.Init;

namespace KPlugin.MaxMediation
{
    public class MaxAdRewarded : MaxAd, IAdRewarded
    {
        #region Properties
        [SerializeField]
        [SelectAdId(MaxAdType.Rewarded)]
        private int indexAd = 0;
        [SerializeField]
        private bool isAutoReload = true;

        private bool isCreated,
            isLoading,
            isLoaded,
            isShow;
        private int attemptLoad;

        public event IAd.OnAdReceivedReward OnAdReceivedRewardEvent;
        private IAd.OnClick onClick;
        private IAd.OnAdReceivedReward onReward;
        private IAd.OnShowComplete onShowComplete;

        public override AdType AdType => AdType.Rewarded;
        public string AdId
        {
            get
            {
                MaxSettingId settingAdId = MaxSetting.Instance.Ad_Get(MaxAdType.Rewarded, indexAd);
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
                if (isAutoReload)
                    Load();
            }
        }
        public override bool IsCreated => isCreated;
        public override bool IsLoading => isLoading;
        public override bool IsLoaded => isLoaded;
        public override bool IsShow => isShow;
        public bool IsReady => MaxSdk.IsRewardedAdReady(AdId);
        #endregion

        #region Unity Event

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
            Reward_EventRegister();
            //
            PushEvent_OnAdCreated(MaxAdType.Rewarded, true);
        }

        public override void Destroy()
        {
            if (!IsCreated)
                return;
            isCreated = false;
            Reward_EventUnRegister();
            //
            PushEvent_OnAdDestroy(MaxAdType.Rewarded);
        }

        public override void Load()
        {
            if (!IsCreated || IsLoading || IsShow)
                return;
            Reward_Loading();
        }


        public void Show(IAd.OnAdReceivedReward onReward, IAd.OnShowComplete onShowComplete = null)
        {
            if (!IsCreated || !IsLoaded || IsLoading || !IsReady || IsShow)
            {
                onReward?.Invoke(AdRewardReceived.RewardFail);
                onShowComplete?.Invoke(false);
                return;
            }
            onClick = null;
            this.onReward = onReward;
            this.onShowComplete = onShowComplete;
            isShow = true;
            MaxSdk.ShowRewardedAd(AdId);
        }

        public void Show(IAd.OnClick onClick, IAd.OnAdReceivedReward onReward, IAd.OnShowComplete onShowComplete = null)
        {
            if (!IsCreated || !IsLoaded || IsLoading || !IsReady || IsShow)
            {
                onReward?.Invoke(AdRewardReceived.RewardFail);
                onShowComplete?.Invoke(false);
                return;
            }
            this.onClick = onClick;
            this.onReward = onReward;
            this.onShowComplete = onShowComplete;
            isShow = true;
            MaxSdk.ShowRewardedAd(AdId);
        }
        #endregion

        #region Max Reward
        private void Reward_EventRegister()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += Reward_OnAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += Reward_OnAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += Reward_OnAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += Reward_OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += Reward_OnAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += Rewarded_OnAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += Reward_OnAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += Reward_OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdReviewCreativeIdGeneratedEvent += Reward_OnAdReviewCreativeIdGeneratedEvent;
        }

        private void Reward_EventUnRegister()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= Reward_OnAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= Reward_OnAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= Reward_OnAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= Reward_OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= Reward_OnAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= Rewarded_OnAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= Reward_OnAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= Reward_OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdReviewCreativeIdGeneratedEvent -= Reward_OnAdReviewCreativeIdGeneratedEvent;
        }

        private void Reward_Loading()
        {
            isLoaded = false;
            isLoading = true;
            StartCoroutine(Reward_IE_Loading(0));
        }
        private IEnumerator Reward_IE_Loading(float delay)
        {
            while (MaxManager.Instance == null || !MaxManager.Instance.InitComplete || delay > 0)
            {
                delay = Mathf.Max(0, delay -= Time.unscaledDeltaTime);
                yield return new WaitForEndOfFrame();
            }
            MaxSdk.LoadRewardedAd(AdId);
        }

        private void Reward_OnAdLoadedEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
        private void Reward_OnAdLoadFailedEvent(string adId, MaxSdkBase.ErrorInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            attemptLoad = Mathf.Min(attemptLoad + 1, 6);
            float delay = Mathf.Pow(2, attemptLoad);
            StartCoroutine(Reward_IE_Loading(delay));
            //
            PushEvent_OnAdLoaded(MaxAdType.Rewarded, false);
        }
        private void Reward_OnAdDisplayedEvent(string adId, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            PushEvent_OnAdDisplayed(MaxAdType.Rewarded, true);
        }
        private void Reward_OnAdDisplayFailedEvent(string adId, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            isShow = false;
            isLoaded = false;
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
            PushEvent_OnAdDisplayed(MaxAdType.Rewarded, false);
            //
            if (IsAutoReload)
                Reward_Loading();
        }
        private void Reward_OnAdClickedEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdClicked(MaxAdType.Rewarded);
        }
        private void Rewarded_OnAdReceivedRewardEvent(string adId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            if (adId != AdId)
                return;
            //
            AdRewardReceived rewardReceived = new AdRewardReceived(reward.Label, reward.Amount);
            Ad_OnReward(rewardReceived);
        }
        private void Reward_OnAdHiddenEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdHidden(MaxAdType.Rewarded);
            //
            if (IsAutoReload)
                Reward_Loading();
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
        private void Reward_OnAdRevenuePaidEvent(string adId, MaxSdkBase.AdInfo adInfo)
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
            PushEvent_OnAdRevenuePaid(MaxAdType.Rewarded, revenuePaid);
        }
        private void Reward_OnAdReviewCreativeIdGeneratedEvent(string adId, string adReviewCreativeId, MaxSdkBase.AdInfo adInfo)
        {

        }
        #endregion
    }
}
