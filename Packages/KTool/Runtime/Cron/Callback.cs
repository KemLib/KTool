using UnityEngine;
using KLibStandard.Concurrent;

namespace KTool.Cron
{
    public abstract class Callback
    {
        #region Properties
        private InterValueBool isComplete;

        public bool IsComplete => isComplete;
        #endregion

        #region Construction
        public Callback()
        {
            isComplete = new (false);
        }
        #endregion

        #region Methods
        internal void SetComplete()
        {
            if (isComplete.TryExchange(true))
                OnComplete();
        }
        protected abstract void OnComplete();
        #endregion
    }
}
