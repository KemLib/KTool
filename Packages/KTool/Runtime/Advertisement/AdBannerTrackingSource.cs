namespace KTool.Advertisement
{
    public class AdBannerTrackingSource : AdBannerTracking
    {
        #region Properties

        #endregion

        #region Contruction
        public AdBannerTrackingSource(string errorMessage) : base(errorMessage)
        {

        }
        public AdBannerTrackingSource() : base()
        {

        }
        #endregion

        #region Event
        public void Displayed(bool isSuccess)
        {
            PushEvent_Displayed(isSuccess);
        }
        public void Expanded(bool isSuccess)
        {
            PushEvent_Expanded(isSuccess);
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
