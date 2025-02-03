using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.MultiThread
{
    public class KTaskFunc<T> : KTask
    {
        #region Properties
        private Task<T> task;
        private Func<T> func;
        private CancellationTokenSource cancellation;
        private object state;
        private UnityAction<Result<T>> onComplete;
        #endregion

        #region Construction
        public KTaskFunc(Func<T> func, object state = null, UnityAction<Result<T>> onComplete = null) : base()
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
                task = new Task<T>(func, cancellation.Token);
            }
            catch (Exception ex)
            {
                if (cancellation != null)
                    cancellation.Dispose();
                Complete_Faulted();
                onComplete?.Invoke(Result<T>.FailException(default(T), ex.Message, state));
            }
        }
        protected override void OnUpdate()
        {
            if (!task.IsCompleted)
                return;
            //
            cancellation.Dispose();
            if (task.IsCompletedSuccessfully)
            {
                Complete_Successfully();
                onComplete?.Invoke(Result<T>.Success(task.Result, state));
            }
            else
            {
                if (task.IsCanceled)
                {
                    Complete_Canceled();
                    onComplete?.Invoke(Result<T>.FailCanceled(default(T), KTASK_CANCELED, state));
                }
                else
                {
                    Complete_Faulted();
                    onComplete?.Invoke(Result<T>.FailException(default(T), task.Exception.Message, state));
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
                }
            }
            //
            Complete_Canceled();
            onComplete?.Invoke(Result<T>.FailCanceled(default(T), KTASK_CANCELED, state));
        }
        #endregion
    }
}
