namespace KTool.Ad
{
    public interface IAdAppOpen : IAd
    {
        #region Properties
        public static IAdAppOpen Instance
        {
            get;
            protected set;
        }

        bool IsReady
        {
            get;
        }
        #endregion

        #region Method
        void Show(OnShowComplete onShowComplete = null);
        void Show(OnClick onClick, OnShowComplete onShowComplete = null);
        #endregion

        #region Instance
        static AdType Instance_AdType => (Instance == null ? AdType.AppOpen : Instance.AdType);
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
        static void Instance_Show(OnShowComplete onShowComplete = null, bool testShowComplete = false)
        {
            if (Instance == null)
            {
#if UNITY_EDITOR
                onShowComplete?.Invoke(testShowComplete);
#else
                onShowComplete?.Invoke(false);
#endif
                return;
            }
            Instance.Show(onShowComplete);
        }
        static void Instance_Show(OnClick onClick, OnShowComplete onShowComplete = null, bool testClick = false, bool testShowComplete = false)
        {
            if (Instance == null)
            {
#if UNITY_EDITOR
                if (testShowComplete && testClick)
                    onClick?.Invoke();
                onShowComplete?.Invoke(testShowComplete);
#else
                onShowComplete?.Invoke(false);
#endif
                return;
            }
            Instance.Show(onClick, onShowComplete);
        }
        #endregion
    }
}
