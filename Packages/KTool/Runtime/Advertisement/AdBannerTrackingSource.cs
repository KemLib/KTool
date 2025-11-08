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
        public void PushEvent_Expanded(bool isSuccess)
        {
            try
            {
                OnAdExpanded?.Invoke(adSource as AdBanner, isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(AdBanner.ERROR_AD_EVENT_EXPANDED_EXCEPTION, AdType.Banner.ToString(), ex.Message));
            }
        }
        #endregion
    }
}
