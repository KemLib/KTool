using UnityEngine;

namespace KTool.Cron
{
    public class ConditionReadTime : Condition
    {
        #region Properties
        private float targetTime;
        #endregion

        #region Construction
        internal ConditionReadTime(float time) : base()
        {
            targetTime = Time.realtimeSinceStartup + Mathf.Max(0, time);
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (Time.realtimeSinceStartup >= targetTime)
                SetComplete();
        }
        #endregion

        #region Build
        public static Condition Create(float time)
        {
            return new ConditionReadTime(time);
        }
        #endregion
    }
}
