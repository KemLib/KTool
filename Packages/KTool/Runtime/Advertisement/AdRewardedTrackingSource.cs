using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public class AdRewardedTrackingSource : AdTrackingSource, IAdRewardedTracking
    {
        #region Properties
        public event AdRewarded.AdReceivedRewardDelegate OnAdReceivedReward;
        #endregion

        #region Contruction
        public AdRewardedTrackingSource(AdRewarded adSource) : base(adSource)
        {

        }
        public AdRewardedTrackingSource(AdRewarded adSource, string errorMessage) : base(adSource, errorMessage)
        {

        }
        #endregion

        #region Event
        public void PushEvent_ReceivedReward(AdRewardReceived adRewardReceived)
        {
            try
            {
                OnAdReceivedReward?.Invoke(adSource as AdRewarded, adRewardReceived);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(AdRewarded.ERROR_AD_EVENT_RECEIVED_REWARD_EXCEPTION, AdType.Rewarded, ex.Message));
            }
        }
        #endregion
    }
}
