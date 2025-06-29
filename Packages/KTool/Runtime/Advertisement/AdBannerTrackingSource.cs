namespace KTool.Advertisement
{
    public class AdBannerTrackingSource : AdBannerTracking
    {
        #region Properties
        private readonly AdBanner adSource;
        #endregion

        #region Contruction
        public AdBannerTrackingSource(string errorMessage) : base(errorMessage)
        {

        }
        public AdBannerTrackingSource(AdBanner adSource) : base()
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
        public void Expanded(bool isSuccess)
        {
            PushEvent_Expanded(adSource, isSuccess);
        }
        #endregion
    }
}
