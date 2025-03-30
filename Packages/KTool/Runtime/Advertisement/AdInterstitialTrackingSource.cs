namespace KTool.Advertisement
{
    public class AdInterstitialTrackingSource : AdInterstitialTracking
    {
        #region Properties

        #endregion

        #region Contruction
        public AdInterstitialTrackingSource(string errorMessage) : base(errorMessage)
        {

        }
        public AdInterstitialTrackingSource() : base()
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
        public void RevenuePaid(AdRevenuePaid adRevenuePaid)
        {
            PushEvent_RevenuePaid(adRevenuePaid);
        }
        #endregion
    }
}
