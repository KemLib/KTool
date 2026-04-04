using UnityEngine;

namespace KTool.Cron
{
    public class ConditionDelegate : Condition
    {
        #region Properties
        public delegate bool CheckConditionDelegate();
        private CheckConditionDelegate checkMethod;
        #endregion

        #region Construction
        internal ConditionDelegate(CheckConditionDelegate checkMethod) : base()
        {
            this.checkMethod = checkMethod;
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (checkMethod != null && checkMethod())
            {
                SetComplete();
                checkMethod = null;
            }
        }
        #endregion

        #region Build
        public static Condition Create(CheckConditionDelegate checkMethod)
        {
            return new ConditionDelegate(checkMethod);
        }
        #endregion
    }
}
