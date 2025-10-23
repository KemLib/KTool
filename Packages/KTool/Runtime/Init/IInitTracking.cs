namespace KTool.Init
{
    public interface IInitTracking : ITracking
    {
        #region Progperties
        public static new IInitTracking Success = new InitTrackingSource(true, true),
            Fail = new InitTrackingSource(true, false);

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
