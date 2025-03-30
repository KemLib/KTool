using KTool.Advertisement.Demo;

namespace KTool.Advertisement
{
    public abstract class AdRewarded : Ad
    {
        #region Properties
        private static AdRewarded instance;
        public static AdRewarded Instance
        {
            get => instance;
            protected set => instance = value;
        }

        public delegate void AdReceivedRewardDelegate(AdRewardReceived rewardReceived);

        public event AdReceivedRewardDelegate OnAdReceivedReward;

        public override AdType AdType => AdType.Rewarded;
        #endregion

        #region Methods
        public abstract AdRewardedTracking Show();
        public static AdRewarded GetInstance()
        {
            return Instance ?? AdRewardedDemo.InstanceAdRewarded;
        }
        #endregion

        #region Event
        protected void PushEvent_ReceivedReward(AdRewardReceived rewardReceived)
        {
            OnAdReceivedReward?.Invoke(rewardReceived);
        }
        #endregion
    }
}
