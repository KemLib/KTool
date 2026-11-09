using KLibStandard.Concurrent;
using UnityEngine;

namespace KTool.Init
{
    public class InitTrackingSource : IInitTracking
    {
        private enum State
        {
            Pending,
            Success,
            Fail
        }
        #region Progperties
        public const int STATE_PENDING = (int)State.Pending;
        public const int STATE_SUCCESS = (int)State.Success;
        public const int STATE_FAIL = (int)State.Fail;
        public const string ERROR_UNKNOWN = "unknown error";

        protected InterValueInt state;
        private InterValueClass<string> errorMessage;
        private readonly bool indispensable;
        private InterValueFloat progress;

        public bool Indispensable => indispensable;
        public bool IsComplete => state.Value != STATE_PENDING;
        public bool IsSuccessfully => state.Value == STATE_SUCCESS;
        public string ErrorMessage
        {
            get => errorMessage;
            private set => errorMessage.Value = value;
        }
        public float Progress
        {
            get => progress;
            set => progress.Value = Mathf.Clamp(value, 0, 1);
        }
        #endregion

        #region Construction
        public InitTrackingSource(bool indispensable)
        {
            this.indispensable = indispensable;
            state = new(STATE_PENDING);
            errorMessage = new InterValueClass<string>(ERROR_UNKNOWN);
            progress = new InterValueFloat(0);
        }
        public InitTrackingSource(bool indispensable, bool isSuccess)
        {
            this.indispensable = indispensable;
            state = new(isSuccess ? STATE_SUCCESS : STATE_FAIL);
            if (isSuccess)
                errorMessage = new InterValueClass<string>(string.Empty);
            else
                errorMessage = new InterValueClass<string>(ERROR_UNKNOWN);
            progress = new InterValueFloat(1);
        }
        public InitTrackingSource(bool indispensable, string errorMessage)
        {
            this.indispensable = indispensable;
            state = new(STATE_FAIL);
            if (string.IsNullOrEmpty(errorMessage))
                this.errorMessage = new InterValueClass<string>(ERROR_UNKNOWN);
            else
                this.errorMessage = new InterValueClass<string>(errorMessage);
            progress = new InterValueFloat(1);
        }
        #endregion

        #region Method
        public bool CompleteSuccess()
        {
            if (!state.TryCompareExchange(STATE_SUCCESS, STATE_PENDING))
                return false;
            //
            errorMessage.Value = string.Empty;
            progress.Value = 1;
            return true;
        }
        public bool CompleteFail()
        {
            if (!state.TryCompareExchange(STATE_FAIL, STATE_PENDING))
                return false;
            //
            errorMessage.Value = ERROR_UNKNOWN;
            progress.Value = 1;
            return true;
        }
        public bool CompleteFail(string errorMessage)
        {
            if (!state.TryCompareExchange(STATE_FAIL, STATE_PENDING))
                return false;
            //
            this.errorMessage.Value = string.IsNullOrEmpty(errorMessage) ? ERROR_UNKNOWN : errorMessage;
            progress.Value = 1;
            return true;
        }
        #endregion
    }
}
