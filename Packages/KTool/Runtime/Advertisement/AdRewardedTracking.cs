namespace KTool.Advertisement
{
    public abstract class AdRewardedTracking
    {
        #region Properties
        public event Ad.AdDisplayedDelegate OnAdDisplayed;
        public event Ad.AdShowCompleteDelegate OnAdShowComplete;
        public event Ad.AdHiddenDelegate OnAdHidden;
        public event Ad.AdClickedDelegate OnAdClicked;
        public event AdRewarded.AdReceivedRewardDelegate OnAdReceivedReward;
        public event Ad.AdRevenuePaidDelegate OnAdRevenuePaid;
        #endregion

        #region Event
        protected void PushEvent_Displayed(bool isSuccess)
        {
            OnAdDisplayed?.Invoke(isSuccess);
        }
        protected void PushEvent_ShowComplete(bool isSuccess)
        {
            OnAdShowComplete?.Invoke(isSuccess);
        }
        protected void PushEvent_Hidden()
        {
            OnAdHidden?.Invoke();
        }
        protected void PushEvent_Clicked()
        {
            OnAdClicked?.Invoke();
        }
        protected void PushEvent_ReceivedReward(AdRewardReceived adRewardReceived)
        {
            OnAdReceivedReward?.Invoke(adRewardReceived);
        }
        protected void PushEvent_RevenuePaid(AdRevenuePaid adRevenuePaid)
        {
            OnAdRevenuePaid?.Invoke(adRevenuePaid);
        }
        #endregion
    }
}
