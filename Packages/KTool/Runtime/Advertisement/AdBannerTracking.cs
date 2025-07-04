using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdBannerTracking : AdTracking
    {
        #region Properties
        public event AdBanner.AdExpandedDelegate OnAdExpanded;
        #endregion

        #region Contruction
        public AdBannerTracking(string errorMessage) : base(errorMessage)
        {

        }
        public AdBannerTracking() : base()
        {

        }
        #endregion

        #region Event
        protected void PushEvent_Expanded(AdBanner adSource, bool isSuccess)
        {
            try
            {
                OnAdExpanded?.Invoke(adSource, isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(AdBanner.ERROR_AD_EVENT_EXPANDED_EXCEPTION, AdType.Banner.ToString(), ex.Message));
            }
        }
        #endregion
    }
}
