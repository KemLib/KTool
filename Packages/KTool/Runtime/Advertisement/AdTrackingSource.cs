using System;
using UnityEngine;

namespace KTool.Advertisement
{
    public class AdTrackingSource : IAdTracking
    {
        #region Properties
        public const string ERROR_UNKNOWN = "unknown error";

        protected readonly AdBase adSource;
        private readonly bool isComplete;
        private readonly string errorMessage;
        private bool isDisplayed,
            isHided;

        public event AdBase.AdDisplayedDelegate OnAdDisplayed;
        public event AdBase.AdHiddenDelegate OnAdHidden;
        public event AdBase.AdClickedDelegate OnAdClicked;
        public event AdBase.AdRevenuePaidDelegate OnAdRevenuePaid;

        public bool IsComplete => isComplete;
        public string ErrorMessage => errorMessage;
        public bool IsDisplayed => isDisplayed;
        public bool IsHided => isHided;
        #endregion

        #region Contruction
        public AdTrackingSource(AdBase adSource)
        {
            this.adSource = adSource;
            isComplete = true;
            errorMessage = ERROR_UNKNOWN;
            isDisplayed = false;
            isHided = false;
        }
        public AdTrackingSource(AdBase adSource, string errorMessage)
        {
            this.adSource = adSource;
            isComplete = false;
            this.errorMessage = string.IsNullOrEmpty(errorMessage) ? ERROR_UNKNOWN : errorMessage;
            isDisplayed = false;
            isHided = true;
        }
        #endregion

        #region Event
        public void PushEvent_Displayed(bool isSuccess, string placement)
        {
            isDisplayed = isSuccess;
            isHided = !isDisplayed;
            //
            OnAdDisplayed?.Invoke(adSource, isSuccess, placement);
        }
        public void PushEvent_Hidden(string placement)
        {
            isDisplayed = false;
            isHided = !isDisplayed;
            //
            OnAdHidden?.Invoke(adSource, placement);
        }
        public void PushEvent_Clicked(string placement)
        {
            OnAdClicked?.Invoke(adSource, placement);
        }
        public void PushEvent_RevenuePaid(AdRevenuePaid adRevenuePaid, string placement)
        {
            OnAdRevenuePaid?.Invoke(adSource, adRevenuePaid, placement);
        }
        #endregion
    }
}
