using KTool.Advertisement.Demo;

namespace KTool.Advertisement
{
    public abstract class AdInterstitial : AdBase
    {
        #region Properties
        protected static AdInterstitial instance;
        public static AdInterstitial Instance => instance == null ? AdDemoInterstitial.InstanceAdDemo : instance;

        private string placement;
        private AdInterstitialTrackingSource trackingSource;

        public override AdType AdType => AdType.Interstitial;
        public string Placement => placement;
        #endregion

        #region Methods
        public IAdTracking Show(string placement = "")
        {
            AdInterstitialTrackingSource trackingSource;
            if (OnShow(out string error))
            {
                trackingSource = new AdInterstitialTrackingSource(this);
                this.placement = string.IsNullOrEmpty(placement) ? AdAppOpen.PLACEMENT_UNKNOWN : placement;
                this.trackingSource = trackingSource;
            }
            else
            {
                trackingSource = new AdInterstitialTrackingSource(this, error);
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
        #endregion
    }
}
