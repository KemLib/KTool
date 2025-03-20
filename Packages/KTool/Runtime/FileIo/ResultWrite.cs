using KLibStandard;

namespace KTool.FileIo
{
    public readonly struct ResultWrite : IResult
    {
        #region Properties
        private readonly bool isSuccess;
        private readonly string errorMessage;
        private readonly object state;

        public readonly bool IsSuccess => isSuccess;
        public readonly string ErrorMessage => errorMessage;
        public readonly object State => state;
        #endregion

        #region Construction
        private ResultWrite(bool isSuccess, string errorMessage, object state)
        {
            this.isSuccess = isSuccess;
            if (isSuccess)
                this.errorMessage = string.Empty;
            else
                this.errorMessage = (string.IsNullOrEmpty(errorMessage) ? IResult.ERROR_MESSAGE_UNKNOWN : errorMessage);
            this.state = state;
        }
        #endregion

        #region Method
        public static ResultWrite Success(object state = null)
        {
            return new ResultWrite(true, string.Empty, state);
        }
        public static ResultWrite Fail(string errorMessage, object state = null)
        {
            return new ResultWrite(false, errorMessage, state);
        }
        #endregion
    }
}
