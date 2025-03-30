namespace KTool.Init
{
    public interface TrackEntry
    {
        #region Progperties
        public static TrackEntry TrackLoaderSuccess = TrackEntrySource.CreateTraskEntrySuccess(),
            TrackLoaderFail = TrackEntrySource.CreateTraskEntryFail();

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
