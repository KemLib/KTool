namespace KTool.Advertisement
{
    public interface IAdRewardedTracking : IAdTracking
    {
        #region Properties
        public event AdRewarded.AdReceivedRewardDelegate OnAdReceivedReward;
        #endregion
    }
}
