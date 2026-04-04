using UnityEngine;

namespace KTool.Cron
{
    public class ConditionFrame : Condition
    {
        #region Properties
        private int number;
        #endregion

        #region Construction
        internal ConditionFrame(int number) : base()
        {
            this.number = Mathf.Max(1, number);
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            number--;
            if (number <= 0)
                SetComplete();
        }
        #endregion

        #region Build
        public static Condition Create(int number)
        {
            return new ConditionFrame(number);
        }
        #endregion
    }
}
