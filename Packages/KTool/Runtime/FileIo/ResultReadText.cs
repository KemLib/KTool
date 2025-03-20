using KLibStandard;

namespace KTool.FileIo
{
    public readonly struct ResultReadText : IResult
    {
        #region Properties
        private readonly bool isSuccess;
        private readonly string errorMessage;
        public readonly string text;
        private readonly object state;

        public readonly bool IsSuccess => isSuccess;
        public readonly string ErrorMessage => errorMessage;
        public readonly string Text => text;
        public readonly object State => state;
        #endregion

        #region Construction
        private ResultReadText(bool isSuccess, string text, string errorMessage, object state)
        {
            this.isSuccess = isSuccess;
            if (isSuccess)
            {
                this.text = text;
                this.errorMessage = string.Empty;
            }
            else
            {
                this.text = string.Empty;
                this.errorMessage = (string.IsNullOrEmpty(errorMessage) ? IResult.ERROR_MESSAGE_UNKNOWN : errorMessage);
            }
            this.state = state;
        }
        #endregion

        #region Method
        public static ResultReadText Success(string data, object state = null)
        {
            return new ResultReadText(true, data, string.Empty, state);
        }
        public static ResultReadText Fail(string errorMessage, object state = null)
        {
            return new ResultReadText(false, string.Empty, errorMessage, state);
        }
        #endregion
    }
}
