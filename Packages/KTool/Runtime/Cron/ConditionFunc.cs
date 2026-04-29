using System;
using UnityEngine;

namespace KTool.Cron
{
    public class ConditionFunc : Condition
    {
        #region Properties
        private Func<bool> method;
        #endregion

        #region Construction
        internal ConditionFunc(Func<bool> method) : base()
        {
            this.method = method;
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if(method == null)
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
        public static Condition Create(Func<bool> checkMethod)
        {
            return new ConditionFunc(checkMethod);
        }
        #endregion
    }
}
