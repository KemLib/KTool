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
            OnAdDisplayed?.Invoke(adSource, isSuccess);
            //
            if (!isSuccess)
                isHided = true;
        }
        public void PushEvent_Hidden()
        {
            OnAdHidden?.Invoke(adSource);
            //
            isHided = true;
        }
        public void PushEvent_Clicked()
        {
            OnAdClicked?.Invoke(adSource);
        }
        public void PushEvent_RevenuePaid(AdRevenuePaid adRevenuePaid)
        {
            OnAdRevenuePaid?.Invoke(adSource, adRevenuePaid);
        }
        #endregion
    }
}
