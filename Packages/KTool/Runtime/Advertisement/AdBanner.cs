using KTool.Advertisement.Demo;
using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdBanner : AdBase
    {
        #region Properties
        protected static AdBanner instance;
        public static AdBanner Instance => instance == null ? AdDemoBanner.InstanceAdDemo : instance;

        public delegate void AdExpandedDelegate(AdBanner source, bool isExpanded, string placement);

        [SerializeField]
        private AdPosition adPosition;
        [SerializeField]
        private AdSize adSize;
        [SerializeField]
        private Vector2 position,
            size;

        private bool isExpanded;
        public event AdExpandedDelegate OnAdExpanded;
        private string placement;
        private AdBannerTrackingSource trackingSource;

        public override AdType AdType => AdType.Banner;
        public virtual AdPosition PositionType
        {
            get => adPosition;
            protected set => adPosition = value;
        }
        public virtual AdSize SizeType
        {
            get => adSize;
            protected set => adSize = value;
        }
        public virtual Vector2 Position
        {
            get => position;
            protected set => position = value;
        }
        public virtual Vector2 Size
        {
            get => size;
            protected set => size = value;
        }
        public virtual bool IsExpanded => IsShow && isExpanded;
        public string Placement => placement;
        #endregion

        #region Methods
        public IAdBannerTracking Show(string placement = "")
        {
            AdBannerTrackingSource trackingSource;
            if (OnShow(out string error))
            {
                trackingSource = new AdBannerTrackingSource(this);
                this.placement = string.IsNullOrEmpty(placement) ? AdAppOpen.PLACEMENT_UNKNOWN : placement;
                this.trackingSource = trackingSource;
            }
            else
            {
                trackingSource = new AdBannerTrackingSource(this, error);
            }
            return trackingSource;
        }
        protected abstract bool OnShow(out string error);
        public void Hide()
        {
            if (OnHide())
                isExpanded = false;
        }
        protected abstract bool OnHide();
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
        protected void PushEvent_Expanded(bool isExpanded)
        {
            this.isExpanded = isExpanded;
            //
            OnAdExpanded?.Invoke(this, isExpanded, placement);
            trackingSource?.PushEvent_Expanded(isExpanded, placement);
        }
        #endregion
    }
}
