using KLibStandard.Concurrent;
using UnityEngine;

namespace KTool.Init
{
    public class InitTrackingSource : IInitTracking
    {
        #region Progperties
        public const string ERROR_UNKNOWN = "unknown error";

        protected InterValueBool isComplete;
        private InterValueBool isSuccessfully;
        private InterValueClass<string> errorMessage;
        private readonly bool indispensable;
        private InterValueFloat progress;

        public bool IsComplete => isComplete;
        public bool IsSuccessfully
        {
            get => isSuccessfully;
            private set => isSuccessfully.Value = value;
        }
        public string ErrorMessage
        {
            get => errorMessage;
            private set => errorMessage.Value = value;
        }
        public bool Indispensable => indispensable;
        public float Progress
        {
            get => progress;
            set => progress.Value = Mathf.Clamp(value, 0, 1);
        }
        #endregion

        #region Construction
        public InitTrackingSource(bool indispensable)
        {
            isComplete = new InterValueBool(false);
            isSuccessfully = new InterValueBool(false);
            errorMessage = new InterValueClass<string>(ERROR_UNKNOWN);
            this.indispensable = indispensable;
            progress = new InterValueFloat(0);
        }
        public InitTrackingSource(bool indispensable, bool isSuccess)
        {
            isComplete = new InterValueBool(true);
            isSuccessfully = new InterValueBool(isSuccess);
            if (isSuccess)
                errorMessage = new InterValueClass<string>(string.Empty);
            else
                errorMessage = new InterValueClass<string>(ERROR_UNKNOWN);
            this.indispensable = indispensable;
            progress = new InterValueFloat(1);
        }
        public InitTrackingSource(bool indispensable, string errorMessage)
        {
            isComplete = new InterValueBool(true);
            isSuccessfully = new InterValueBool(false);
            if (string.IsNullOrEmpty(errorMessage))
                this.errorMessage = new InterValueClass<string>(ERROR_UNKNOWN);
            else
                this.errorMessage = new InterValueClass<string>(errorMessage);
            this.indispensable = indispensable;
            progress = new InterValueFloat(1);
        }
        #endregion

        #region Method
        public bool CompleteSuccess()
        {
            if (isComplete.Exchange(true))
                return false;
            //
            IsSuccessfully = true;
            ErrorMessage = string.Empty;
            Progress = 1;
            return true;
        }
        public bool CompleteFail()
        {
            if (isComplete.Exchange(true))
                return false;
            //
            IsSuccessfully = false;
            ErrorMessage = ERROR_UNKNOWN;
            Progress = 1;
            return true;
        }
        public bool CompleteFail(string errorMessage)
        {
            if (isComplete.Exchange(true))
                return false;
            //
            IsSuccessfully = false;
            ErrorMessage = string.IsNullOrEmpty(errorMessage) ? ERROR_UNKNOWN : errorMessage;
            Progress = 1;
            return true;
        }
        #endregion
    }
}
