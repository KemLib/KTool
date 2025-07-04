namespace KTool.Advertisement
{
    public class AdRewardedTrackingSource : AdRewardedTracking
    {
        #region Properties
        private readonly AdRewarded adSource;
        #endregion

        #region Contruction
        public AdRewardedTrackingSource(string errorMessage) : base(errorMessage)
        {

        }
        public AdRewardedTrackingSource(AdRewarded adSource) : base()
        {
            this.adSource = adSource;
        }
        #endregion

        #region Event
        public void Displayed(bool isSuccess)
        {
            PushEvent_Displayed(adSource, isSuccess);
        }
        public void Hidden()
        {
            PushEvent_Hidden(adSource);
        }
        public void Clicked()
        {
            PushEvent_Clicked(adSource);
        }
        public void RevenuePaid(AdRevenuePaid adRevenuePaid)
        {
            PushEvent_RevenuePaid(adSource, adRevenuePaid);
        }
        public void ReceivedReward(AdRewardReceived adRewardReceived)
        {
            PushEvent_ReceivedReward(adSource, adRewardReceived);
        }
        #endregion
    }
}
