﻿namespace KTool.Advertisement
{
    public abstract class AdBannerTracking
    {
        #region Properties
        private const string ERROR_UNKNOWN = "unknown error";
        public readonly bool IsShow;
        public readonly string ErrorMessage;

        public event Ad.AdDisplayedDelegate OnAdDisplayed;
        public event AdBanner.AdExpandedDelegate OnAdExpanded;
        public event Ad.AdShowCompleteDelegate OnAdShowComplete;
        public event Ad.AdHiddenDelegate OnAdHidden;
        public event Ad.AdClickedDelegate OnAdClicked;
        public event Ad.AdRevenuePaidDelegate OnAdRevenuePaid;
        #endregion

        #region Contruction
        public AdBannerTracking(string errorMessage)
        {
            IsShow = false;
            ErrorMessage = string.IsNullOrEmpty(errorMessage) ? ERROR_UNKNOWN : errorMessage;
        }
        public AdBannerTracking()
        {
            IsShow = false;
            ErrorMessage = string.Empty;
        }
        #endregion

        #region Event
        protected void PushEvent_Displayed(bool isSuccess)
        {
            OnAdDisplayed?.Invoke(isSuccess);
        }
        protected void PushEvent_Expanded(bool isSuccess)
        {
            OnAdExpanded?.Invoke(isSuccess);
        }
        protected void PushEvent_ShowComplete(bool isSuccess)
        {
            OnAdShowComplete?.Invoke(isSuccess);
        }
        protected void PushEvent_Hidden()
        {
            OnAdHidden?.Invoke();
        }
        protected void PushEvent_Clicked()
        {
            OnAdClicked?.Invoke();
        }
        protected void PushEvent_RevenuePaid(AdRevenuePaid adRevenuePaid)
        {
            OnAdRevenuePaid?.Invoke(adRevenuePaid);
        }
        #endregion
    }
}
