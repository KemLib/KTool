namespace KTool.Init
{
    public interface IInitTracking : ITracking
    {
        #region Progperties
        public static new IInitTracking Success = new InitTrackingSource(false, true),
            Fail = new InitTrackingSource(false, false);

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
