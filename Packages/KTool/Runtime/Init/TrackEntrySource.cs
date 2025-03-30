using KLibStandard.Concurrent;
using UnityEngine;

namespace KTool.Init
{
    public class TrackEntrySource : TrackEntry
    {
        #region Progperties
        public const string ERROR_UNKNOWN = "unknown error";

        private InterValueFloat progress;
        private InterValueBool isComplete,
            isSuccessfully;
        private InterValueClass<string> errorMessage;

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
        public TrackEntrySource()
        {
            progress = new InterValueFloat(0);
            isComplete = new InterValueBool(false);
            isSuccessfully = new InterValueBool(false);
            errorMessage = new InterValueClass<string>(string.Empty);
        }
        #endregion

        #region Method
        public void CompleteSuccess()
        {
            Progress = 1;
            IsComplete = true;
            IsSuccessfully = true;
            ErrorMessage = string.Empty;
        }
        public void CompleteFail()
        {
            Progress = 1;
            IsComplete = true;
            IsSuccessfully = false;
            ErrorMessage = ERROR_UNKNOWN;
        }
        public void CompleteFail(string errorMessage)
        {
            Progress = 1;
            IsComplete = true;
            IsSuccessfully = false;
            ErrorMessage = string.IsNullOrEmpty(errorMessage) ? ERROR_UNKNOWN : errorMessage;
        }
        public static TrackEntry CreateTraskEntrySuccess()
        {
            TrackEntrySource trackLoaderSource = new TrackEntrySource();
            trackLoaderSource.CompleteSuccess();
            return trackLoaderSource;
        }
        public static TrackEntry CreateTraskEntryFail()
        {
            TrackEntrySource trackLoaderSource = new TrackEntrySource();
            trackLoaderSource.CompleteFail();
            return trackLoaderSource;
        }
        public static TrackEntry CreateTraskEntryFail(string errorMessage)
        {
            TrackEntrySource trackLoaderSource = new TrackEntrySource();
            trackLoaderSource.CompleteFail(errorMessage);
            return trackLoaderSource;
        }
        #endregion
    }
}
