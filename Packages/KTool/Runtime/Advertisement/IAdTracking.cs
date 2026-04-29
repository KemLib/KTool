namespace KTool.Advertisement
{
    public interface IAdTracking
    {
        #region Properties
        public event AdBase.AdDisplayedDelegate OnAdDisplayed;
        public event AdBase.AdHiddenDelegate OnAdHidden;
        public event AdBase.AdClickedDelegate OnAdClicked;
        public event AdBase.AdRevenuePaidDelegate OnAdRevenuePaid;

        public bool IsComplete
        {
            get;
        }
        public string ErrorMessage
        {
            get;
        }
        public bool IsDisplayed
        {
            get;
        }
        public bool IsHided
        {
            get;
        }
        #endregion
    }
}
