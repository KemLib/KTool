namespace KTool
{
    public struct Result<T> : IResult
    {
        #region Properties
        private readonly T value;
        private readonly bool isSuccess,
            isCanceled,
            isException;
        private readonly string error;
        private readonly object state;

        public T Value => value;
        public bool IsSuccess => isSuccess;
        public bool IsCanceled => isCanceled;
        public bool IsException => isException;
        public string Error => error;
        public object State => state;
        #endregion

        #region Construction
        private Result(T value, object state)
        {
            this.value = value;
            isSuccess = true;
            isCanceled = false;
            isException = false;
            error = IResult.ERROR_NO_ERROR;
            this.state = state;
        }
        private Result(T value, bool isCanceled, bool isException, string error, object state)
        {
            this.value = value;
            isSuccess = false;
            this.isCanceled = isCanceled;
            this.isException = isException;
            this.error = (string.IsNullOrEmpty(error) ? IResult.ERROR_UNKNOWN : error);
            this.state = state;
        }
        #endregion

        #region Method
        public static Result<T> Success(T value, object state = null)
        {
            return new Result<T>(value, state);
        }
        public static Result<T> FailCanceled(T value, string error = null, object state = null)
        {
            return new Result<T>(value, true, false, error, state);
        }
        public static Result<T> FailException(T value, string error = null, object state = null)
        {
            return new Result<T>(value, false, true, error, state);
        }
        #endregion
    }
}
