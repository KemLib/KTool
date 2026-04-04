using UnityEngine;

namespace KTool.Cron
{
    public abstract class Condition
    {
        #region Properties
        private bool isComplete;

        public bool IsComplete => isComplete;
        #endregion

        #region Construction
        public Condition()
        {
            isComplete = false;
        }
        #endregion

        #region Methods
        internal void Check()
        {
            if (isComplete)
                return;
            OnCheck();
        }
        protected abstract void OnCheck();
        protected void SetComplete()
        {
            isComplete = true;
        }
        #endregion
    }
}
