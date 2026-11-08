using UnityEngine;
using KLibStandard.Concurrent;

namespace KTool.Cron
{
    public abstract class Condition
    {
        #region Properties
        private InterValueBool isComplete;

        public bool IsComplete => isComplete;
        #endregion

        #region Construction
        public Condition()
        {
            isComplete = new (false);
        }
        #endregion

        #region Methods
        internal void Check()
        {
            if (isComplete.Value)
                return;
            OnCheck();
        }
        protected abstract void OnCheck();
        protected void SetComplete()
        {
            isComplete.Value = true;
        }
        #endregion
    }
}
