using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public class AdTrackingSource : TrackingSource, IAdTracking
    {
        #region Properties
        protected readonly Ad adSource;

        public event Ad.AdDisplayedDelegate OnAdDisplayed;
        public event Ad.AdHiddenDelegate OnAdHidden;
        public event Ad.AdClickedDelegate OnAdClicked;
        public event Ad.AdRevenuePaidDelegate OnAdRevenuePaid;
        #endregion

        #region Contruction
        public AdTrackingSource(Ad adSource) : base()
        {
            this.adSource = adSource;
        }
        public AdTrackingSource(Ad adSource, string errorMessage) : base(errorMessage)
        {
            this.adSource = adSource;
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
                CompleteFail();
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
            CompleteSuccess();
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
