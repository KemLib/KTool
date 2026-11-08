using UnityEngine;

namespace KTool.Cron
{
    public class ConditionFrame : Condition
    {
        #region Properties
        private int targetFrame;
        #endregion

        #region Construction
        internal ConditionFrame(int number) : base()
        {
            targetFrame = Time.frameCount + Mathf.Max(0, number);
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (Time.frameCount >= targetFrame)
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
