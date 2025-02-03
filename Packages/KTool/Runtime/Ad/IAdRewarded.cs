namespace KTool.Ad
{
    public interface IAdRewarded : IAd
    {
        #region Properties
        public static IAdRewarded Instance => AdManager.Instance.Get<IAdRewarded>(0);

        event OnAdReceivedReward OnAdReceivedRewardEvent;

        bool IsReady
        {
            get;
        }
        #endregion

        #region Method
        void Show(OnAdReceivedReward onReward, OnShowComplete onShowComplete = null);
        void Show(OnClick onClick, OnAdReceivedReward onReward, OnShowComplete onShowComplete = null);
        #endregion

        #region Instance
        static AdType Instance_AdType => (Instance == null ? AdType.Rewarded : Instance.AdType);
        static bool Instance_IsCreated => (Instance == null ? false : Instance.IsCreated);
        static bool Instance_IsLoaded => (Instance == null ? false : Instance.IsLoaded);
        static bool Instance_IsLoading => (Instance == null ? false : Instance.IsLoading);
        static bool Instance_IsShow => (Instance == null ? false : Instance.IsShow);
        static bool Instance_IsAutoReload
        {
            get => (Instance == null ? false : Instance.IsAutoReload);
            set
            {
                if (Instance == null)
                    return;
                Instance.IsAutoReload = value;
            }
        }
        static bool Instance_IsReady => (Instance == null ? false : Instance.IsReady);

        static void Instance_Create()
        {
            if (Instance == null)
                return;
            Instance.Create();
        }
        static void Instance_Load()
        {
            if (Instance == null)
                return;
            Instance.Load();
        }
        static void Instance_Destroy()
        {
            if (Instance == null)
                return;
            Instance.Destroy();
        }
        static void Instance_Show(OnAdReceivedReward onReward, OnShowComplete onShowComplete = null, bool testReward = false, bool testShowComplete = false)
        {
            if (Instance == null)
            {
#if UNITY_EDITOR
                if (testShowComplete && testReward)
                    onReward?.Invoke(AdRewardReceived.RewardSuccess);
                else
                    onReward?.Invoke(AdRewardReceived.RewardFail);
                onReward?.Invoke(AdRewardReceived.RewardFail);
                onShowComplete?.Invoke(testShowComplete);
#else
                onReward?.Invoke(AdRewardReceived.RewardFail);
                onShowComplete?.Invoke(false);
#endif
                return;
            }
            Instance.Show(onReward, onShowComplete);
        }
        static void Instance_Show(OnClick onClick, OnAdReceivedReward onReward, OnShowComplete onShowComplete = null, bool testReward = false, bool testShowComplete = false)
        {
            if (Instance == null)
            {
#if UNITY_EDITOR
                if (testShowComplete && testReward)
                    onClick?.Invoke();
                if (testShowComplete && testReward)
                    onReward?.Invoke(AdRewardReceived.RewardSuccess);
                else
                    onReward?.Invoke(AdRewardReceived.RewardFail);
                onShowComplete?.Invoke(testShowComplete);
#else
                onReward?.Invoke(AdRewardReceived.RewardFail);
                onShowComplete?.Invoke(false);
#endif
                return;
            }
            Instance.Show(onClick, onReward, onShowComplete);
        }
        #endregion
    }
}
