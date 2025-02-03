namespace KTool
{
    public struct Result : IResult
    {
        #region Properties
        private readonly bool isSuccess,
            isCanceled,
            isException;
        private readonly string error;
        private readonly object state;

        public bool IsSuccess => isSuccess;
        public bool IsCanceled => isCanceled;
        public bool IsException => isException;
        public string Error => error;
        public object State => state;
        #endregion

        #region Construction
        private Result(object state)
        {
            isSuccess = true;
            isCanceled = false;
            isException = false;
            error = IResult.ERROR_NO_ERROR;
            this.state = state;
        }
        private Result(bool isCanceled, bool isException, string error, object state)
        {
            isSuccess = false;
            this.isCanceled = isCanceled;
            this.isException = isException;
            this.error = (string.IsNullOrEmpty(error) ? IResult.ERROR_UNKNOWN : error);
            this.state = state;
        }
        #endregion

        #region Method
        public static Result Success(object state = null)
        {
            return new Result(state);
        }
        public static Result FailCanceled(string error = null, object state = null)
        {
            return new Result(true, false, error, state);
        }
        public static Result FailException(string error = null, object state = null)
        {
            return new Result(false, true, error, state);
        }
        #endregion
    }
}
