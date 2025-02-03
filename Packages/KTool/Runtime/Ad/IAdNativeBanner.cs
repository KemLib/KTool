namespace KTool.Ad
{
    public interface IAdNativeBanner : IAd
    {
        #region Properties
        bool IsReady
        {
            get;
        }
        #endregion

        #region Method
        void Show(AdNativePanel panel);
        #endregion
    }
}
