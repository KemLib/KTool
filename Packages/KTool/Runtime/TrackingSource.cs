using KLibStandard.Concurrent;

namespace KTool
{
    public class TrackingSource : ITracking
    {
        #region Progperties
        public const string ERROR_UNKNOWN = "unknown error";

        protected InterValueBool isComplete;
        private InterValueBool isSuccessfully;
        private InterValueClass<string> errorMessage;

        public bool IsComplete => isComplete;
        public bool IsSuccessfully
        {
            get => isSuccessfully;
            private set => isSuccessfully.Value = value;
        }
        public string ErrorMessage
        {
            get => errorMessage;
            private set => errorMessage.Value = string.IsNullOrEmpty(value) ? ERROR_UNKNOWN : value;
        }
        #endregion

        #region Construction
        public TrackingSource()
        {
            isComplete = new InterValueBool(false);
            isSuccessfully = new InterValueBool(false);
            errorMessage = new InterValueClass<string>(string.Empty);
        }
        public TrackingSource(bool isSuccess)
        {
            isComplete = new InterValueBool(true);
            isSuccessfully = new InterValueBool(isSuccess);
            if (isSuccess)
                errorMessage = new InterValueClass<string>(string.Empty);
            else
                errorMessage = new InterValueClass<string>(ERROR_UNKNOWN);
        }
        public TrackingSource(string errorMessage)
        {
            isComplete = new InterValueBool(true);
            isSuccessfully = new InterValueBool(false);
            if (string.IsNullOrEmpty(errorMessage))
                this.errorMessage = new InterValueClass<string>(ERROR_UNKNOWN);
            else
                this.errorMessage = new InterValueClass<string>(errorMessage);
        }
        #endregion

        #region Method
        public virtual bool CompleteSuccess()
        {
            if (isComplete.Exchange(true))
                return false;
            //
            SetSuccess();
            return true;
        }
        public virtual bool CompleteFail()
        {
            if (isComplete.Exchange(true))
                return false;
            //
            SetFail();
            return true;
        }
        public virtual bool CompleteFail(string errorMessage)
        {
            if (isComplete.Exchange(true))
                return false;
            //
            SetFail(errorMessage);
            return true;
        }
        protected void SetSuccess()
        {
            IsSuccessfully = true;
            ErrorMessage = string.Empty;
        }
        protected void SetFail()
        {
            IsSuccessfully = false;
            ErrorMessage = ERROR_UNKNOWN;
        }
        protected void SetFail(string errorMessage)
        {
            IsSuccessfully = false;
            ErrorMessage = errorMessage;
        }
        #endregion
    }
}
