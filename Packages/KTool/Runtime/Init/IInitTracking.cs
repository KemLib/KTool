namespace KTool.Init
{
    public interface IInitTracking
    {
        #region Progperties
        public static IInitTracking Success = new InitTrackingSource(true, true),
            Fail = new InitTrackingSource(true, false);

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
        public bool Indispensable
        {
            get;
        }
        public float Progress
        {
            get;
        }
        #endregion
    }
}
