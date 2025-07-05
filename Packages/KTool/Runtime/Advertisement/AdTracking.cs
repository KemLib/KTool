using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public abstract class AdTracking
    {
        #region Properties
        private const string ERROR_UNKNOWN = "unknown error";

        public readonly bool IsShow;
        public readonly string ErrorMessage;

        public event Ad.AdDisplayedDelegate OnAdDisplayed;
        public event Ad.AdHiddenDelegate OnAdHidden;
        public event Ad.AdClickedDelegate OnAdClicked;
        public event Ad.AdRevenuePaidDelegate OnAdRevenuePaid;
        #endregion

        #region Contruction
        public AdTracking(string errorMessage)
        {
            IsShow = false;
            ErrorMessage = string.IsNullOrEmpty(errorMessage) ? ERROR_UNKNOWN : errorMessage;
        }
        public AdTracking()
        {
            IsShow = true;
            ErrorMessage = string.Empty;
        }
        #endregion

        #region Event
        protected void PushEvent_Displayed(Ad adSource, bool isSuccess)
        {
            try
            {
                OnAdDisplayed?.Invoke(adSource, isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_DISPLAYED_EXCEPTION, AdType.Interstitial, ex.Message));
            }
        }
        protected void PushEvent_Hidden(Ad adSource)
        {
            try
            {
                OnAdHidden?.Invoke(adSource);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_HIDDEN_EXCEPTION, AdType.Interstitial, ex.Message));
            }
        }
        protected void PushEvent_Clicked(Ad adSource)
        {
            try
            {
                OnAdClicked?.Invoke(adSource);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_CLICKED_EXCEPTION, AdType.Interstitial, ex.Message));
            }
        }
        protected void PushEvent_RevenuePaid(Ad adSource, AdRevenuePaid adRevenuePaid)
        {
            try
            {
                OnAdRevenuePaid?.Invoke(adSource, adRevenuePaid);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_REVENUE_PAID_EXCEPTION, AdType.Interstitial, ex.Message));
            }
        }
        #endregion
    }
}
