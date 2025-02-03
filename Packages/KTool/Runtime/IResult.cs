namespace KTool
{
    public interface IResult
    {
        #region Properties
        public const string ERROR_UNKNOWN = "Unknown",
            ERROR_NO_ERROR = "No_Error";

        bool IsSuccess
        {
            get;
        }
        bool IsCanceled
        {
            get;
        }
        bool IsException
        {
            get;
        }
        string Error
        {
            get;
        }
        object State
        {
            get;
        }
        #endregion

        #region Method

        #endregion
    }
}
