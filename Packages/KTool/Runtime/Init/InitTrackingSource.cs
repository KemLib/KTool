using KLibStandard.Concurrent;
using UnityEngine;

namespace KTool.Init
{
    public class InitTrackingSource : TrackingSource, IInitTracking
    {
        #region Progperties
        private readonly bool indispensable;
        private InterValueFloat progress;

        public bool Indispensable => indispensable;
        public float Progress
        {
            get => progress;
            set => progress.Value = Mathf.Clamp(value, 0, 1);
        }
        #endregion

        #region Construction
        public InitTrackingSource(bool indispensable) : base()
        {
            this.indispensable = indispensable;
            progress = new InterValueFloat(0);
        }
        public InitTrackingSource(bool indispensable, bool isSuccess) : base(isSuccess)
        {
            this.indispensable = indispensable;
            progress = new InterValueFloat(1);
        }
        public InitTrackingSource(bool indispensable, string errorMessage) : base(errorMessage)
        {
            this.indispensable = indispensable;
            progress = new InterValueFloat(1);
        }
        #endregion

        #region Method
        public override bool CompleteSuccess()
        {
            if (isComplete.Exchange(true))
                return false;
            //
            Progress = 1;
            SetSuccess();
            return true;
        }
        public override bool CompleteFail()
        {
            if (isComplete.Exchange(true))
                return false;
            //
            Progress = 1;
            SetFail();
            return true;
        }
        public override bool CompleteFail(string errorMessage)
        {
            if (isComplete.Exchange(true))
                return false;
            //
            Progress = 1;
            SetFail(errorMessage);
            return true;
        }
        #endregion
    }
}
