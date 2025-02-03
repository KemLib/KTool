using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace KTool.MultiThread
{
    public abstract class KTask : IKTask
    {
        #region Properties
        public const string KTASK_CANCELED = "KTask Canceled";

        private int tokenRun,
            tokenComplete,
            tokenCanceled,
            tokenFaulted,
            tokenSuccessfully;

        public bool IsRun
        {
            get => Interlocked.CompareExchange(ref tokenRun, 0, 0) == 1;
            protected set => Interlocked.Exchange(ref tokenRun, (value ? 1 : 0));
        }
        public bool IsComplete
        {
            get => Interlocked.CompareExchange(ref tokenComplete, 0, 0) == 1;
            private set => Interlocked.Exchange(ref tokenComplete, (value ? 1 : 0));
        }
        public bool IsCanceled
        {
            get => Interlocked.CompareExchange(ref tokenCanceled, 0, 0) == 1;
            private set => Interlocked.Exchange(ref tokenCanceled, (value ? 1 : 0));
        }
        public bool IsFaulted
        {
            get => Interlocked.CompareExchange(ref tokenFaulted, 0, 0) == 1;
            private set => Interlocked.Exchange(ref tokenFaulted, (value ? 1 : 0));
        }
        public bool IsSuccessfully
        {
            get => Interlocked.CompareExchange(ref tokenSuccessfully, 0, 0) == 1;
            private set => Interlocked.Exchange(ref tokenSuccessfully, (value ? 1 : 0));
        }
        #endregion

        #region Construction
        public KTask()
        {
            tokenRun = 0;
            tokenComplete = 0;
            tokenCanceled = 0;
            tokenFaulted = 0;
            tokenSuccessfully = 0;
        }
        #endregion

        #region Method
        public void Start()
        {
            if (IsComplete || IsRun)
                return;
            //
            IsRun = true;
            OnStart();
        }
        public void Update()
        {
            if (IsComplete || !IsRun)
                return;
            //
            OnUpdate();
        }
        public void Stop()
        {
            if (IsComplete)
                return;
            //
            OnStop();
        }
        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnStop();
        #endregion

        #region Complete
        protected void Complete_Canceled()
        {
            IsRun = false;
            IsComplete = true;
            IsCanceled = true;
            IsFaulted = false;
            IsSuccessfully = false;
        }
        protected void Complete_Faulted()
        {
            IsRun = false;
            IsComplete = true;
            IsCanceled = false;
            IsFaulted = true;
            IsSuccessfully = false;
        }
        protected void Complete_Successfully()
        {
            IsRun = false;
            IsComplete = true;
            IsCanceled = false;
            IsFaulted = false;
            IsSuccessfully = true;
        }
        #endregion
    }
}
