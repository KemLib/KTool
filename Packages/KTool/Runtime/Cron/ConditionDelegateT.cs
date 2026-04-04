using UnityEngine;

namespace KTool.Cron
{
    public class ConditionDelegate<T> : Condition
    {
        #region Properties
        public delegate bool CheckConditionDelegate(T state);
        private CheckConditionDelegate checkMethod;
        private T state;
        #endregion

        #region Construction
        internal ConditionDelegate(CheckConditionDelegate checkMethod, T state) : base()
        {
            this.checkMethod = checkMethod;
            this.state = checkMethod == null ? default : state;
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (checkMethod != null && checkMethod(state))
            {
                SetComplete();
                checkMethod = null;
                state = default;
            }
        }
        #endregion

        #region Build
        public static Condition Create(CheckConditionDelegate checkMethod, T state)
        {
            return new ConditionDelegate<T>(checkMethod, state);
        }
        #endregion
    }
}
