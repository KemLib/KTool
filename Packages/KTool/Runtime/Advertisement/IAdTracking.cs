namespace KTool.Advertisement
{
    public interface IAdTracking : ITracking
    {
        #region Properties
        public event Ad.AdDisplayedDelegate OnAdDisplayed;
        public event Ad.AdHiddenDelegate OnAdHidden;
        public event Ad.AdClickedDelegate OnAdClicked;
        public event Ad.AdRevenuePaidDelegate OnAdRevenuePaid;
        #endregion
    }
}
