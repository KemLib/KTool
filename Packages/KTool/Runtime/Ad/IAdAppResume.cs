namespace KTool.Ad
{
    public interface IAdAppResume
    {
        #region Properties
        public static IAdAppResume Instance
        {
            get;
            protected set;
        }

        bool IsActive
        {
            get;
            set;
        }

        event IAd.OnAdDisplayed OnAdDisplayed;
        event IAd.OnAdHidden OnAdHidden;
        #endregion

        #region Method

        #endregion

        #region Instance
        static bool Instance_IsActive
        {
            get => (Instance == null ? false : Instance.IsActive);
            set
            {
                if (Instance == null)
                    return;
                Instance.IsActive = value;
            }
        }
        #endregion
    }
}
