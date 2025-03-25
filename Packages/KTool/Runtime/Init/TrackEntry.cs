namespace KTool.Init
{
    public interface TrackEntry
    {
        #region Progperties
        public static TrackEntry TrackLoaderSuccess = TrackEntrySource.CreateTraskEntrySuccess(string.Empty),
            TrackLoaderFail = TrackEntrySource.CreateTraskEntryFail(string.Empty);

        public string Name
        {
            get;
        }
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
