using System;

namespace KTool.MainThreadSafe
{
    internal class ActionT3<T1, T2, T3> : IAction
    {
        #region Properties
        private readonly Action<T1, T2, T3> action;
        private readonly T1 state1;
        private readonly T2 state2;
        private readonly T3 state3;
        #endregion

        #region Constructions
        public ActionT3(Action<T1, T2, T3> action, T1 state1, T2 state2, T3 state3)
        {
            this.action = action;
            this.state1 = state1;
            this.state2 = state2;
            this.state3 = state3;
        }
        #endregion

        #region Methods
        public void Execute()
        {
            action?.Invoke(state1, state2, state3);
        }
        #endregion
    }
}
