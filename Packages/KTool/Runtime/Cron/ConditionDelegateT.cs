using UnityEngine;
using System;

namespace KTool.Cron
{
    public class ConditionDelegate<T> : Condition
    {
        #region Properties
        public delegate bool CheckConditionDelegate(T state);
        private CheckConditionDelegate method;
        private T state;
        #endregion

        #region Construction
        internal ConditionDelegate(CheckConditionDelegate method, T state) : base()
        {
            this.method = method;
            this.state = state;
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (method == null)
            {
                SetComplete();
                state = default;
                return;
            }
            //
            bool isDone = false;
            try{
                isDone = method(state);
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }
            //
            if (isDone)
            {
                SetComplete();
                method = null;
                state = default;
            }
        }
        #endregion
    }
}
