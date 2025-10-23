using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public class AdTrackingSource : IAdTracking
    {
        #region Properties
        public const string ERROR_UNKNOWN = "unknown error";

        protected readonly Ad adSource;
        private readonly bool isComplete;
        private readonly string errorMessage;
        private bool isHided;

        public event Ad.AdDisplayedDelegate OnAdDisplayed;
        public event Ad.AdHiddenDelegate OnAdHidden;
        public event Ad.AdClickedDelegate OnAdClicked;
        public event Ad.AdRevenuePaidDelegate OnAdRevenuePaid;

        public bool IsComplete => isComplete;
        public string ErrorMessage => errorMessage;
        public bool IsHided => isHided;
        #endregion

        #region Contruction
        public AdTrackingSource(Ad adSource) : base()
        {
            this.adSource = adSource;
            isComplete = true;
            errorMessage = ERROR_UNKNOWN;
            isHided = false;
        }
        public AdTrackingSource(Ad adSource, string errorMessage)
        {
            this.adSource = adSource;
            isComplete = false;
            errorMessage = string.IsNullOrEmpty(errorMessage) ? ERROR_UNKNOWN : errorMessage;
            isHided = true;
        }
        #endregion

        #region Event
        public void PushEvent_Displayed(bool isSuccess)
        {
            try
            {
                OnAdDisplayed?.Invoke(adSource, isSuccess);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_DISPLAYED_EXCEPTION, AdType.Interstitial, ex.Message));
            }
            //
            if (!isSuccess)
                isHided = true;
        }
        public void PushEvent_Hidden()
        {
            try
            {
                OnAdHidden?.Invoke(adSource);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format(Ad.ERROR_AD_EVENT_HIDDEN_EXCEPTION, AdType.Interstitial, ex.Message));
            }
            //
            isHided = true;
        }
        public void PushEvent_Clicked()
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
        public void PushEvent_RevenuePaid(AdRevenuePaid adRevenuePaid)
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
