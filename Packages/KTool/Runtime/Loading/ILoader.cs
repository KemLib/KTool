namespace KTool.Loading
{
    public interface ILoader
    {
        #region Progperties

        #endregion

        #region Method
        public TrackEntry LoadBegin();
        public void LoadEnd();
        #endregion
    }
}
