using UnityEngine;

namespace KTool.Worker
{
    public interface IWorker
    {
        #region Properties
        public float Progress
        {
            get;
        }
        public bool IsDone
        {
            get;
        }
        public bool IsSuccess
        {
            get;
        }
        public bool IsCanceled
        {
            get;
        }
        public bool IsFailed
        {
            get;
        }
        public bool IsTimeout
        {
            get;
        }
        #endregion

        #region Methods
        public void Cancel();
        #endregion
    }
}
