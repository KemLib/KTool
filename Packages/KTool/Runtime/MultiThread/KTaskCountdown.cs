using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.MultiThread
{
    public class KTaskCountdown : KTask
    {
        #region Properties
        private float timeOrigin,
            timeCurrent;
        private object state;
        private UnityAction<Result> onComplete;

        public float TimeCountdown => timeCurrent;
        #endregion

        #region Construction
        public KTaskCountdown(float time, object state = null, UnityAction<Result> onComplete = null) : base()
        {
            timeOrigin = time;
            this.state = state;
            this.onComplete = onComplete;
        }
        #endregion

        #region Method
        public void Restart()
        {
            timeCurrent = timeOrigin;
        }
        protected override void OnStart()
        {
            if (timeOrigin <= 0)
            {
                Complete_Successfully();
                onComplete?.Invoke(Result.Success(state));
            }
            else
            {
                timeCurrent = timeOrigin;
            }
        }
        protected override void OnUpdate()
        {
            timeCurrent -= Time.deltaTime;
            if (timeCurrent <= 0)
            {
                Complete_Successfully();
                onComplete?.Invoke(Result.Success(state));
            }
        }
        protected override void OnStop()
        {
            Complete_Canceled();
            onComplete?.Invoke(Result.FailCanceled(KTASK_CANCELED, state));
        }
        #endregion
    }
}
