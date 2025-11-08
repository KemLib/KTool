namespace KTool.Init
{
    public interface IIniter
    {
        #region Properties

        #endregion

        #region Method
        public IInitTracking InitBegin();
        public void InitEnd();
        #endregion
    }
}
