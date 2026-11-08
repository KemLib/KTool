using System;

namespace KTool.MainThreadSafe
{
    internal class ActionT2<T1, T2> : IAction
    {
        #region Properties
        private readonly Action<T1, T2> action;
        private readonly T1 state1;
        private readonly T2 state2;
        #endregion

        #region Constructions
        public ActionT2(Action<T1, T2> action, T1 state1, T2 state2)
        {
            this.action = action;
            this.state1 = state1;
            this.state2 = state2;
        }
        #endregion

        #region Methods
        public void Execute()
        {
            action?.Invoke(state1, state2);
        }
        #endregion
    }
}
