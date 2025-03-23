namespace KTool.Advertisement
{
    public struct AdRewardReceived
    {
        #region Properties
        public static AdRewardReceived RewardFail = new AdRewardReceived(string.Empty, false, 1),
            RewardSuccess = new AdRewardReceived(string.Empty, true, 1);

        public readonly string Label;
        public readonly bool IsReward;
        public readonly double Value;
        #endregion

        #region Construction
        public AdRewardReceived(string label, bool isReward, double value)
        {
            IsReward = isReward;
            Label = label;
            Value = value;
        }
        #endregion

        #region Method

        #endregion
    }
}
