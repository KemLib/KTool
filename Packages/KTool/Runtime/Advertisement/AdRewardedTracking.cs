using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdRewardedTracking
    {
        #region Properties
        internal const string ERROR_AD_EVENT_RECEIVED_REWARD_EXCEPTION = "Ad {0} call event ReceivedReward exception: {1}";
        private const string ERROR_UNKNOWN = "unknown error";

        public readonly bool IsShow;
        public readonly string ErrorMessage;

        public event Ad.AdDisplayedDelegate OnAdDisplayed;
        public event Ad.AdShowCompleteDelegate OnAdShowComplete;
        public event Ad.AdHiddenDelegate OnAdHidden;
        public event Ad.AdClickedDelegate OnAdClicked;
        public event AdRewarded.AdReceivedRewardDelegate OnAdReceivedReward;
        public event Ad.AdRevenuePaidDelegate OnAdRevenuePaid;
        #endregion

        #region Contruction
        public AdRewardedTracking(string errorMessage)
        {
            IsShow = false;
            ErrorMessage = string.IsNullOrEmpty(errorMessage) ? ERROR_UNKNOWN : errorMessage;
        }
        public AdRewardedTracking()
        {
            IsShow = true;
            ErrorMessage = string.Empty;
        }
        #endregion

        #region Event
        protected void PushEvent_Displayed(bool isSuccess)
        {
            try
            {
                OnAdDisplayed?.Invoke(isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_DISPLAYED_EXCEPTION, AdType.Rewarded.ToString(), ex.Message));
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
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_SHOW_COMPLETE_EXCEPTION, AdType.Rewarded.ToString(), ex.Message));
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
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_HIDDEN_EXCEPTION, AdType.Rewarded.ToString(), ex.Message));
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
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_CLICKED_EXCEPTION, AdType.Rewarded.ToString(), ex.Message));
            }
        }
        protected void PushEvent_RevenuePaid(AdRevenuePaid adRevenuePaid)
        {
            try
            {
                OnAdRevenuePaid?.Invoke(adRevenuePaid);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_REVENUEPAID_EXCEPTION, AdType.Rewarded.ToString(), ex.Message));
            }
        }
        protected void PushEvent_ReceivedReward(AdRewardReceived adRewardReceived)
        {
            try
            {
                OnAdReceivedReward?.Invoke(adRewardReceived);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_RECEIVED_REWARD_EXCEPTION, AdType.Rewarded.ToString(), ex.Message));
            }
        }
        #endregion
    }
}
