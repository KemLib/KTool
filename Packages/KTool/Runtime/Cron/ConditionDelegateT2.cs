using UnityEngine;
using System;

namespace KTool.Cron
{
    public class ConditionDelegate<T1, T2> : Condition
    {
        #region Properties
        public delegate bool CheckConditionDelegate(T1 state1, T2 state2);
        private CheckConditionDelegate method;
        private T1 state1;
        private T2 state2;
        #endregion

        #region Construction
        internal ConditionDelegate(CheckConditionDelegate method, T1 state1, T2 state2) : base()
        {
            this.method = method;
            this.state1 = state1;
            this.state2 = state2;
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (method == null)
            {
                SetComplete();
                state1 = default;
                state2 = default;
                return;
            }
            //
            bool isDone = false;
            try{
                isDone = method(state1, state2);
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
                state1 = default;
                state2 = default;
            }
        }
        #endregion
    }
}
