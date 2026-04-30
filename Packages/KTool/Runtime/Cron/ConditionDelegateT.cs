using UnityEngine;

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
            if (method(state))
            {
                SetComplete();
                method = null;
                state = default;
            }
        }
        #endregion
    }
}
