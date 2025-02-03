namespace KTool.Ad
{
    public interface IAd
    {
        #region Properties
        public delegate void OnAdCreated(bool isSuccess);
        public delegate void OnAdLoaded(bool isSuccess);
        public delegate void OnAdDisplayed(bool isSuccess);
        public delegate void OnAdClicked();
        public delegate void OnAdReceivedReward(AdRewardReceived rewardReceived);
        public delegate void OnAdHidden();
        public delegate void OnAdDestroy();
        public delegate void OnAdRevenuePaid(AdRevenuePaid adRevenuePaid);
        public delegate void OnAdExpanded(bool isExpanded);
        public delegate void OnClick();
        public delegate void OnShowComplete(bool isSuccess);

        event OnAdCreated OnAdCreatedEvent;
        event OnAdLoaded OnAdLoadedEvent;
        event OnAdDisplayed OnAdDisplayedEvent;
        event OnAdClicked OnAdClickedEvent;
        event OnAdHidden OnAdHiddenEvent;
        event OnAdDestroy OnAdDestroyEvent;
        event OnAdRevenuePaid OnAdRevenuePaidEvent;

        string Name
        {
            get;
        }
        AdType AdType
        {
            get;
        }
        bool IsCreated
        {
            get;
        }
        bool IsLoaded
        {
            get;
        }
        bool IsLoading
        {
            get;
        }
        bool IsShow
        {
            get;
        }
        bool IsAutoReload
        {
            get;
            set;
        }
        #endregion

        #region Method
        void Create();
        void Load();
        void Destroy();
        #endregion
    }
}
