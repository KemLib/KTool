using System;
using System.Collections.Generic;

namespace KTool.MultiThread
{
    public interface IKTask
    {
        #region Properties
        public bool IsRun
        {
            get;
        }
        public bool IsComplete
        {
            get;
        }
        public bool IsCanceled
        {
            get;
        }
        public bool IsFaulted
        {
            get;
        }
        public bool IsSuccessfully
        {
            get;
        }
        #endregion

        #region Method
        public void Stop();
        #endregion
    }
}
