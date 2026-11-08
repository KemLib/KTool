using System;

namespace KTool.MainThreadSafe
{
    internal class ActionT0 : IAction
    {
        #region Properties
        private readonly Action action;
        #endregion

        #region Constructions
        public ActionT0(Action action)
        {
            this.action = action;
        }
        #endregion

        #region Methods
        public void Execute()
        {
            action?.Invoke();
        }
        #endregion
    }
}
