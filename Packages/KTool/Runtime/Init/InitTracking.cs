namespace KTool.Init
{
    public interface InitTracking
    {
        #region Progperties
        public static InitTracking Success = InitTrackingSource.CreateSuccess(),
            Fail = InitTrackingSource.CreateFail();

        public float Progress
        {
            get;
        }
        public bool IsComplete
        {
            get;
        }
        public bool IsSuccessfully
        {
            get;
        }
        public string ErrorMessage
        {
            get;
        }
        #endregion

        #region Method

        #endregion
    }
}
