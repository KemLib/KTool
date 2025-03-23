using KLibStandard;

namespace KTool.FileIo
{
    public readonly struct ResultRead : IResult
    {
        #region Properties
        private static readonly byte[] DATA_EMPTY = new byte[0];

        private readonly bool isSuccess;
        private readonly string errorMessage;
        public readonly byte[] data;
        private readonly object state;

        public readonly bool IsSuccess => isSuccess;
        public readonly string ErrorMessage => errorMessage;
        public readonly byte[] Data => data;
        public readonly object State => state;
        #endregion

        #region Construction
        private ResultRead(bool isSuccess, byte[] data, string errorMessage, object state)
        {
            this.isSuccess = isSuccess;
            if (isSuccess)
            {
                this.data = data;
                this.errorMessage = string.Empty;
            }
            else
            {
                this.data = DATA_EMPTY;
                this.errorMessage = (string.IsNullOrEmpty(errorMessage) ? IResult.ERROR_MESSAGE_UNKNOWN : errorMessage);
            }
            this.state = state;
        }
        #endregion

        #region Method
        public static ResultRead Success(byte[] data, object state = null)
        {
            return new ResultRead(true, data, string.Empty, state);
        }
        public static ResultRead Fail(string errorMessage, object state = null)
        {
            return new ResultRead(false, DATA_EMPTY, errorMessage, state);
        }
        #endregion
    }
}
