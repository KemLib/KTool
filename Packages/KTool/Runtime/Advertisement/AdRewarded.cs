using KTool.Advertisement.Demo;
using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdRewarded : AdBase
    {
        #region Properties
        protected static AdRewarded instance;
        public static AdRewarded Instance => instance == null ? AdDemoRewarded.InstanceAdDemo : instance;

        private string placement;
        private AdRewardedTrackingSource trackingSource;

        public delegate void AdReceivedRewardDelegate(AdRewarded source, AdRewardReceived rewardReceived, string placement);
        public event AdReceivedRewardDelegate OnAdReceivedReward;

        public override AdType AdType => AdType.Rewarded;
        public string Placement => placement;
        #endregion

        #region Methods
        public IAdRewardedTracking Show(string placement = "")
        {
            AdRewardedTrackingSource trackingSource;
            if(OnShow(out string error))
            {
                trackingSource = new AdRewardedTrackingSource(this);
                this.placement = string.IsNullOrEmpty(placement) ? AdAppOpen.PLACEMENT_UNKNOWN : placement;
                this.trackingSource = trackingSource;
            }
            else
            {
                trackingSource = new AdRewardedTrackingSource(this, error);
            }
            return trackingSource;
        }
        protected abstract bool OnShow(out string error);
        #endregion

        #region Event
        protected void PushEvent_Displayed(bool isSuccess)
        {
            PushEvent_Displayed(isSuccess, placement);
            trackingSource?.PushEvent_Displayed(isSuccess, placement);
        }
        protected void PushEvent_Hidden()
        {
            PushEvent_Hidden(placement);
            trackingSource?.PushEvent_Hidden(placement);
        }
        protected void PushEvent_Clicked()
        {
            PushEvent_Clicked(placement);
            trackingSource?.PushEvent_Clicked(placement);
        }
        protected void PushEvent_RevenuePaid(AdRevenuePaid revenuePaid)
        {
            PushEvent_RevenuePaid(revenuePaid, placement);
            trackingSource?.PushEvent_RevenuePaid(revenuePaid, placement);
        }
        protected void PushEvent_ReceivedReward(AdRewardReceived rewardReceived)
        {
            OnAdReceivedReward?.Invoke(this, rewardReceived, placement);
        }
        #endregion
    }
}
