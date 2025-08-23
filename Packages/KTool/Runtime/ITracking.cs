namespace KTool
{
    public interface ITracking
    {
        #region Progperties
        public static ITracking Success = new TrackingSource(true),
            Fail = new TrackingSource(false);

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
