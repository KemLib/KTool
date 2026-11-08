using System;

namespace KTool.MainThreadSafe
{
    internal class ActionT1<T> : IAction
    {
        #region Properties
        private readonly Action<T> action;
        private readonly T state;
        #endregion

        #region Constructions
        public ActionT1(Action<T> action, T state)
        {
            this.action = action;
            this.state = state;
        }
        #endregion

        #region Methods
        public void Execute()
        {
            action?.Invoke(state);
        }
        #endregion
    }
}
