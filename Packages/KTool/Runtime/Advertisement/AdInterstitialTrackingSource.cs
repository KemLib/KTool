namespace KTool.Advertisement
{
    public class AdInterstitialTrackingSource : AdInterstitialTracking
    {
        #region Properties
        private readonly AdInterstitial adSource;
        #endregion

        #region Contruction
        public AdInterstitialTrackingSource(string errorMessage) : base(errorMessage)
        {

        }
        public AdInterstitialTrackingSource(AdInterstitial adSource) : base()
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
        #endregion
    }
}
