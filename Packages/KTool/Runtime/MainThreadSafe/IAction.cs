using KLibStandard.Concurrent.Queue;
using System;

namespace KTool.MainThreadSafe
{
    public interface IAction
    {
        #region Properties

        #endregion

        #region Methods
        public void Execute();
        #endregion
    }
}
