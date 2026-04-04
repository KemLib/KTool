using UnityEngine;

namespace KTool.Cron
{
    public abstract class Callback
    {
        #region Properties
        private bool isComplete;

        public bool IsComplete => isComplete;
        #endregion

        #region Construction
        public Callback()
        {

        }
        #endregion

        #region Methods
        internal void SetComplete()
        {
            if (isComplete)
                return;
            isComplete = true;
            OnComplete();
        }
        protected abstract void OnComplete();
        #endregion
    }
}
