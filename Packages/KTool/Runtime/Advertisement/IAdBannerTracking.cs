namespace KTool.Advertisement
{
    public interface IAdBannerTracking : IAdTracking
    {
        #region Properties
        public event AdBanner.AdExpandedDelegate OnAdExpanded;
        #endregion
    }
}
