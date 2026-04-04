using UnityEngine;

namespace KTool.Cron
{
    public class ConditionReadTime : Condition
    {
        #region Properties
        private float time;
        #endregion

        #region Construction
        internal ConditionReadTime(float time) : base()
        {
            this.time = Mathf.Max(0, time);
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            time = Mathf.Max(0, time - Time.unscaledDeltaTime);
            if (time <= 0)
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
