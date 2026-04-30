using KTool.Advertisement.Demo;

namespace KTool.Advertisement
{
    public abstract class AdAppOpen : AdBase
    {
        #region Properties
        public const string PLACEMENT_UNKNOWN = "unknown_placement";

        protected static AdAppOpen instance;
        public static AdAppOpen Instance => instance == null ? AdDemoAppOpen.InstanceAdDemo : instance;

        private string placement;
        private AdAppOpenTrackingSource trackingSource;

        public override AdType AdType => AdType.AppOpen;
        public string Placement => placement;
        #endregion

        #region Methods
        public IAdTracking Show(string placement = "")
        {
            string oldPlacement = placement;
            this.placement = string.IsNullOrEmpty(placement) ? PLACEMENT_UNKNOWN : placement;
            //
            AdAppOpenTrackingSource trackingSource;
            if (OnShow(out string error))
            {
                trackingSource = new AdAppOpenTrackingSource(this);
                this.trackingSource = trackingSource;
            }
            else
            {
                trackingSource = new AdAppOpenTrackingSource(this, error);
                this.placement = oldPlacement;
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
