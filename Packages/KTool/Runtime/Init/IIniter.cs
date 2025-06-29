namespace KTool.Init
{
    public interface IIniter
    {
        #region Properties
        public bool RequiredConditions
        {
            get;
        }
        #endregion

        #region Method
        public InitTracking InitBegin();
        public void InitEnd();
        #endregion
    }
}
