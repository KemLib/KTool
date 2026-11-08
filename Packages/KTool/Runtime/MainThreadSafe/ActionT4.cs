using System;

namespace KTool.MainThreadSafe
{
    internal class ActionT4<T1, T2, T3, T4> : IAction
    {
        #region Properties
        private readonly Action<T1, T2, T3, T4> action;
        private readonly T1 state1;
        private readonly T2 state2;
        private readonly T3 state3;
        private readonly T4 state4;
        #endregion

        #region Constructions
        public ActionT4(Action<T1, T2, T3, T4> action, T1 state1, T2 state2, T3 state3, T4 state4)
        {
            this.action = action;
            this.state1 = state1;
            this.state2 = state2;
            this.state3 = state3;
            this.state4 = state4;
        }
        #endregion

        #region Methods
        public void Execute()
        {
            action?.Invoke(state1, state2, state3, state4);
        }
        #endregion
    }
}
