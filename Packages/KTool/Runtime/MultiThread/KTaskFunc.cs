using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.MultiThread
{
    public class KTaskFunc : KTask
    {
        #region Properties
        private Task task;
        private Action func;
        private CancellationTokenSource cancellation;
        private object state;
        private UnityAction<Result> onComplete;
        #endregion

        #region Construction
        public KTaskFunc(Action func, object state = null, UnityAction<Result> onComplete = null) : base()
        {
            this.func = func;
            this.state = state;
            this.onComplete = onComplete;
        }
        #endregion

        #region Method
        protected override void OnStart()
        {
            try
            {
                cancellation = new CancellationTokenSource();
                task = new Task(func, cancellation.Token);
            }
            catch (Exception ex)
            {
                if (cancellation != null)
                {
                    cancellation.Dispose();
                    cancellation = null;
                }
                Complete_Faulted();
                onComplete?.Invoke(Result.FailException(ex.Message, state));
            }
        }
        protected override void OnUpdate()
        {
            if (!task.IsCompleted)
                return;
            //
            cancellation.Dispose();
            cancellation = null;
            if (task.IsCompletedSuccessfully)
            {
                Complete_Successfully();
                onComplete?.Invoke(Result.Success(state));
            }
            else
            {
                if (task.IsCanceled)
                {
                    Complete_Canceled();
                    onComplete?.Invoke(Result.FailCanceled(KTASK_CANCELED, state));
                }
                else
                {
                    Complete_Faulted();
                    onComplete?.Invoke(Result.FailException(task.Exception.Message, state));
                }
            }
        }
        protected override void OnStop()
        {
            if (cancellation != null)
            {
                try
                {
                    cancellation.Cancel();
                }
                catch (Exception)
                {

                }
                finally
                {
                    cancellation.Dispose();
                    cancellation = null;
                }
            }
            //
            Complete_Canceled();
            onComplete?.Invoke(Result.FailCanceled(KTASK_CANCELED, state));
        }
        #endregion
    }
}
