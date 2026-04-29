using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public class AdBannerTrackingSource : AdTrackingSource, IAdBannerTracking
    {
        #region Properties
        public event AdBanner.AdExpandedDelegate OnAdExpanded;
        #endregion

        #region Contruction
        public AdBannerTrackingSource(AdBanner adSource) : base(adSource)
        {

        }
        public AdBannerTrackingSource(AdBanner adSource, string errorMessage) : base(adSource, errorMessage)
        {

        }
        #endregion

        #region Event
        public void PushEvent_Expanded(bool isSuccess, string placement)
        {
            OnAdExpanded?.Invoke(adSource as AdBanner, isSuccess, placement);
        }
        #endregion
    }
}
