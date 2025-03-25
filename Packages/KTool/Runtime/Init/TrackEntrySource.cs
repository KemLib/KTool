using KLibStandard.Concurrent;
using UnityEngine;

namespace KTool.Init
{
    public class TrackEntrySource : TrackEntry
    {
        #region Progperties
        public const string ERROR_MESSAGE_CANCELED = "Task Canceled";

        private string name;
        private InterValueFloat progress;
        private InterValueBool isComplete,
            isSuccessfully;
        private InterValueClass<string> errorMessage;

        public string Name
        {
            get => name;
            set => name = value ?? string.Empty;
        }
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
        public TrackEntrySource(string name)
        {
            this.name = name;
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
            ErrorMessage = string.Empty;
        }
        public void CompleteFail(string errorMessage)
        {
            Progress = 1;
            IsComplete = true;
            IsSuccessfully = false;
            ErrorMessage = errorMessage;
        }
        public static TrackEntry CreateTraskEntrySuccess(string name)
        {
            TrackEntrySource trackLoaderSource = new TrackEntrySource(name);
            trackLoaderSource.CompleteSuccess();
            return trackLoaderSource;
        }
        public static TrackEntry CreateTraskEntryFail(string name)
        {
            TrackEntrySource trackLoaderSource = new TrackEntrySource(name);
            trackLoaderSource.CompleteFail();
            return trackLoaderSource;
        }
        public static TrackEntry CreateTraskEntryFail(string name, string errorMessage)
        {
            TrackEntrySource trackLoaderSource = new TrackEntrySource(name);
            trackLoaderSource.CompleteFail(errorMessage);
            return trackLoaderSource;
        }
        #endregion
    }
}
