namespace KTool.Advertisement
{
    public class AdRewardedTrackingSource : AdRewardedTracking
    {
        #region Properties

        #endregion

        #region Contruction
        public AdRewardedTrackingSource(string errorMessage) : base(errorMessage)
        {

        }
        public AdRewardedTrackingSource() : base()
        {

        }
        #endregion

        #region Event
        public void Displayed(bool isSuccess)
        {
            PushEvent_Displayed(isSuccess);
        }
        public void ShowComplete(bool isSuccess)
        {
            PushEvent_ShowComplete(isSuccess);
        }
        public void Hidden()
        {
            PushEvent_Hidden();
        }
        public void Clicked()
        {
            PushEvent_Clicked();
        }
        public void ReceivedReward(AdRewardReceived adRewardReceived)
        {
            PushEvent_ReceivedReward(adRewardReceived);
        }
        public void RevenuePaid(AdRevenuePaid adRevenuePaid)
        {
            PushEvent_RevenuePaid(adRevenuePaid);
        }
        #endregion
    }
}
