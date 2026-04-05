using UnityEngine;

namespace KTool.Cron
{
    public class ConditionReadTime : Condition
    {
        #region Properties
        private float tagetTime;
        #endregion

        #region Construction
        internal ConditionReadTime(float time) : base()
        {
            tagetTime = Time.time + Mathf.Max(0, time);
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (Time.time >= tagetTime)
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
