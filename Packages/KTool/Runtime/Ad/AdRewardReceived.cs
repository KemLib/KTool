namespace KTool.Ad
{
    public struct AdRewardReceived
    {
        #region Properties
        public static AdRewardReceived RewardFail = new AdRewardReceived(),
            RewardSuccess = new AdRewardReceived(string.Empty, 1);

        private bool isReward;
        private string label;
        private double value;

        public bool IsReward => isReward;
        public string Label => label;
        public double Value => value;
        #endregion

        #region Construction
        public AdRewardReceived(string label, double value)
        {
            isReward = true;
            this.label = label;
            this.value = value;
        }
        #endregion

        #region Method

        #endregion
    }
}
