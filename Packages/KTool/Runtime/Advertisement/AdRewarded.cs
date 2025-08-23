using KTool.Advertisement.Demo;
using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdRewarded : Ad
    {
        #region Properties
        internal const string ERROR_AD_EVENT_RECEIVED_REWARD_EXCEPTION = "Ad {0} call event ReceivedReward exception: {1}";

        protected static AdRewarded instance;
        public static AdRewarded Instance => instance == null ? AdDemoRewarded.InstanceAdDemo : instance;

        public delegate void AdReceivedRewardDelegate(AdRewarded source, AdRewardReceived rewardReceived);

        public event AdReceivedRewardDelegate OnAdReceivedReward;

        public override AdType AdType => AdType.Rewarded;
        #endregion

        #region Methods
        public abstract IAdRewardedTracking Show();
        #endregion

        #region Event
        protected void PushEvent_ReceivedReward(AdRewardReceived rewardReceived)
        {
            try
            {
                OnAdReceivedReward?.Invoke(this, rewardReceived);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(ERROR_AD_EVENT_RECEIVED_REWARD_EXCEPTION, AdType.Rewarded, ex.Message));
            }
        }
        #endregion
    }
}
