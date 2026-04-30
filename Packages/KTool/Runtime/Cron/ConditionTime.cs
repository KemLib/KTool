using System.Diagnostics;
using UnityEngine;

namespace KTool.Cron
{
    public class ConditionTime : Condition
    {
        #region Properties
        private int startFrame;
        private float time;
        #endregion

        #region Construction
        internal ConditionTime(float time) : base()
        {
            startFrame = Time.frameCount;
            this.time = Mathf.Max(0, time);
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (time <= 0)
            {
                SetComplete();
                return;
            }
            //
            if (Time.frameCount <= startFrame)
                return;
            time -= Time.deltaTime;
            if (time <= 0)
                SetComplete();
        }
        #endregion

        #region Build
        public static Condition Create(float time)
        {
            return new ConditionTime(time);
        }
        #endregion
    }
}
