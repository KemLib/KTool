using UnityEngine;

namespace KTool.Cron
{
    public class ConditionDelegate : Condition
    {
        #region Properties
        public delegate bool CheckConditionDelegate();
        private CheckConditionDelegate method;
        #endregion

        #region Construction
        internal ConditionDelegate(CheckConditionDelegate method) : base()
        {
            this.method = method;
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (method == null)
            {
                SetComplete();
                return;
            }
            //
            if (method())
            {
                SetComplete();
                method = null;
            }
        }
        #endregion

        #region Build
        public static Condition Create(CheckConditionDelegate checkMethod)
        {
            return new ConditionDelegate(checkMethod);
        }
        public static Condition Create<T>(ConditionDelegate<T>.CheckConditionDelegate checkMethod, T state)
        {
            return new ConditionDelegate<T>(checkMethod, state);
        }
        public static Condition Create<T1, T2>(ConditionDelegate<T1, T2>.CheckConditionDelegate checkMethod, T1 state1, T2 state2)
        {
            return new ConditionDelegate<T1, T2>(checkMethod, state1, state2);
        }
        public static Condition Create<T1, T2, T3>(ConditionDelegate<T1, T2, T3>.CheckConditionDelegate checkMethod, T1 state1, T2 state2, T3 state3)
        {
            return new ConditionDelegate<T1, T2, T3>(checkMethod, state1, state2, state3);
        }
        public static Condition Create<T1, T2, T3, T4>(ConditionDelegate<T1, T2, T3, T4>.CheckConditionDelegate checkMethod, T1 state1, T2 state2, T3 state3, T4 state4)
        {
            return new ConditionDelegate<T1, T2, T3, T4>(checkMethod, state1, state2, state3, state4);
        }
        #endregion
    }
}
