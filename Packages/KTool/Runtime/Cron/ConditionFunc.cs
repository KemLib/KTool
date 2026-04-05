using System;
using UnityEngine;

namespace KTool.Cron
{
    public class ConditionFunc : Condition
    {
        #region Properties
        private Func<bool> checkMethod;
        #endregion

        #region Construction
        internal ConditionFunc(Func<bool> checkMethod) : base()
        {
            this.checkMethod = checkMethod;
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if(checkMethod == null)
            {
                SetComplete();
                return;
            }
            //
            if (checkMethod())
            {
                SetComplete();
                checkMethod = null;
            }
        }
        #endregion

        #region Build
        public static Condition Create(Func<bool> checkMethod)
        {
            return new ConditionFunc(checkMethod);
        }
        #endregion
    }
}
