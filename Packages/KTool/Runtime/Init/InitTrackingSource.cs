using KLibStandard.Concurrent;
using UnityEngine;

namespace KTool.Init
{
    public class InitTrackingSource : InitTracking
    {
        #region Progperties
        public const string ERROR_UNKNOWN = "unknown error";

        private readonly bool indispensable;
        private InterValueFloat progress;
        private InterValueBool isComplete,
            isSuccessfully;
        private InterValueClass<string> errorMessage;

        public bool Indispensable => indispensable;
        public float Progress
        {
            get => progress;
            set => progress.Value = Mathf.Clamp(value, 0, 1);
        }
        public bool IsComplete
        {
            get => isComplete;
            private set => isComplete.Value = value;
        }
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
        #endregion

        #region Construction
        public InitTrackingSource(bool indispensable)
        {
            this.indispensable = indispensable;
            progress = new InterValueFloat(0);
            isComplete = new InterValueBool(false);
            isSuccessfully = new InterValueBool(false);
            errorMessage = new InterValueClass<string>(string.Empty);
        }
        private InitTrackingSource(bool indispensable, string errorMessage)
        {
            this.indispensable = indispensable;
            progress = new InterValueFloat(1);
            isComplete = new InterValueBool(true);
            isSuccessfully = new InterValueBool(false);
            errorMessage = new InterValueClass<string>(errorMessage);
        }
        private InitTrackingSource(bool indispensable, bool isSuccess)
        {
            this.indispensable = indispensable;
            progress = new InterValueFloat(1);
            isComplete = new InterValueBool(true);
            isSuccessfully = new InterValueBool(isSuccess);
            errorMessage = new InterValueClass<string>(ERROR_UNKNOWN);
        }
        #endregion

        #region Method
        public void CompleteSuccess()
        {
            if (IsComplete)
                return;
            //
            Progress = 1;
            IsComplete = true;
            IsSuccessfully = true;
            ErrorMessage = string.Empty;
        }
        public void CompleteFail()
        {
            if (IsComplete)
                return;
            //
            Progress = 1;
            IsComplete = true;
            IsSuccessfully = false;
            ErrorMessage = ERROR_UNKNOWN;
        }
        public void CompleteFail(string errorMessage)
        {
            if (IsComplete)
                return;
            //
            Progress = 1;
            IsComplete = true;
            IsSuccessfully = false;
            ErrorMessage = string.IsNullOrEmpty(errorMessage) ? ERROR_UNKNOWN : errorMessage;
        }
        public static InitTracking CreateSuccess()
        {
            InitTrackingSource trackSource = new InitTrackingSource(false, true);
            return trackSource;
        }
        public static InitTracking CreateFail()
        {
            InitTrackingSource trackSource = new InitTrackingSource(false, false);
            return trackSource;
        }
        public static InitTracking CreateFail(string errorMessage)
        {
            InitTrackingSource trackSource = new InitTrackingSource(false, errorMessage);
            return trackSource;
        }
        #endregion
    }
}
