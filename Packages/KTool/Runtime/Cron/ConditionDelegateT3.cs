using UnityEngine;

namespace KTool.Cron
{
    public class ConditionDelegate<T1, T2, T3> : Condition
    {
        #region Properties
        public delegate bool CheckConditionDelegate(T1 state1, T2 state2, T3 state3);
        private CheckConditionDelegate method;
        private T1 state1;
        private T2 state2;
        private T3 state3;
        #endregion

        #region Construction
        internal ConditionDelegate(CheckConditionDelegate method, T1 state1, T2 state2, T3 state3) : base()
        {
            this.method = method;
            this.state1 = state1;
            this.state2 = state2;
            this.state3 = state3;
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
                state3 = default;
                return;
            }
            //
            if (method(state1, state2, state3))
            {
                SetComplete();
                method = null;
                state1 = default;
                state2 = default;
                state3 = default;
            }
        }
        #endregion
    }
}
