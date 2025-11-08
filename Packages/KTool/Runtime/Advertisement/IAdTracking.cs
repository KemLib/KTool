namespace KTool.Advertisement
{
    public interface IAdTracking
    {
        #region Properties
        public event Ad.AdDisplayedDelegate OnAdDisplayed;
        public event Ad.AdHiddenDelegate OnAdHidden;
        public event Ad.AdClickedDelegate OnAdClicked;
        public event Ad.AdRevenuePaidDelegate OnAdRevenuePaid;

        public bool IsComplete
        {
            get;
        }
        public string ErrorMessage
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
