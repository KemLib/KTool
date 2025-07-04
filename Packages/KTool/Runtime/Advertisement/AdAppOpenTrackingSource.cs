namespace KTool.Advertisement
{
    public class AdAppOpenTrackingSource : AdAppOpenTracking
    {
        #region Properties
        private readonly AdAppOpen adSource;
        #endregion

        #region Contruction
        public AdAppOpenTrackingSource(string errorMessage) : base(errorMessage)
        {

        }
        public AdAppOpenTrackingSource(AdAppOpen adSource) : base()
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
