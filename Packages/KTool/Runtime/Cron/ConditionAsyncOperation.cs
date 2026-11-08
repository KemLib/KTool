using UnityEngine;

namespace KTool.Cron
{
    public class ConditionAsyncOperation : Condition
    {
        #region Properties
        private AsyncOperation asyncOperation;
        private float targetProgress;
        #endregion

        #region Construction
        internal ConditionAsyncOperation(AsyncOperation asyncOperation) : base()
        {
            this.asyncOperation = asyncOperation;
            targetProgress = -1;
        }
        internal ConditionAsyncOperation(AsyncOperation asyncOperation, float tagetProgress) : base()
        {
            this.asyncOperation = asyncOperation;
            this.targetProgress = Mathf.Clamp(tagetProgress, 0, 1);
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (asyncOperation == null)
            {
                SetComplete();
                return;
            }
            //
            if (targetProgress <= -1)
            {
                if (asyncOperation.isDone)
                {
                    SetComplete();
                    asyncOperation = null;
                }
            }
            else
            {
                if (asyncOperation.isDone || asyncOperation.progress >= targetProgress)
                {
                    SetComplete();
                    asyncOperation = null;
                }
            }
        }
        #endregion

        #region Build
        public static Condition Create(AsyncOperation asyncOperation)
        {
            return new ConditionAsyncOperation(asyncOperation);
        }
        public static Condition Create(AsyncOperation asyncOperation, float tagetProgress)
        {
            return new ConditionAsyncOperation(asyncOperation, tagetProgress);
        }
        #endregion
    }
}
