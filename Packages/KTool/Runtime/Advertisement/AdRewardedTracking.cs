using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdRewardedTracking : AdTracking
    {
        #region Properties
        public event AdRewarded.AdReceivedRewardDelegate OnAdReceivedReward;
        #endregion

        #region Contruction
        public AdRewardedTracking(string errorMessage) : base(errorMessage)
        {

        }
        public AdRewardedTracking() : base()
        {

        }
        #endregion

        #region Event
        protected void PushEvent_ReceivedReward(AdRewarded adSource, AdRewardReceived adRewardReceived)
        {
            try
            {
                OnAdReceivedReward?.Invoke(adSource, adRewardReceived);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(AdRewarded.ERROR_AD_EVENT_RECEIVED_REWARD_EXCEPTION, AdType.Rewarded, ex.Message));
            }
        }
        #endregion
    }
}
