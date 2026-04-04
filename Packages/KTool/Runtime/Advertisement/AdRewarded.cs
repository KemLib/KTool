using KTool.Advertisement.Demo;
using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdRewarded : Ad
    {
        #region Properties
        protected static AdRewarded instance;
        public static AdRewarded Instance => instance == null ? AdDemoRewarded.InstanceAdDemo : instance;

        public delegate void AdReceivedRewardDelegate(AdRewarded source, AdRewardReceived rewardReceived);

        public event AdReceivedRewardDelegate OnAdReceivedReward;

        public override AdType AdType => AdType.Rewarded;
        #endregion

        #region Methods
        public abstract IAdRewardedTracking Show(string placement = "");
        #endregion

        #region Event
        protected void PushEvent_ReceivedReward(AdRewardReceived rewardReceived)
        {
            OnAdReceivedReward?.Invoke(this, rewardReceived);
        }
        #endregion
    }
}
