namespace KTool.Ad
{
    public interface IAdNative : IAd
    {
        #region Properties
        bool IsReady
        {
            get;
        }
        #endregion

        #region Method        
        void Show(AdNativePanel nativePanel);
        #endregion
    }
}
