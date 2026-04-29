using UnityEngine;

namespace KTool.Cron
{
    public class CallbackAction<T1, T2, T3> : Callback
    {
        #region Properties
        public delegate void ActionDelegate(T1 state1, T2 state2, T3 state3);
        private ActionDelegate onAction;
        private T1 state1;
        private T2 state2;
        private T3 state3;
        #endregion

        #region Construction
        internal CallbackAction(ActionDelegate onAction, T1 state1, T2 state2, T3 state3) : base()
        {
            this.onAction = onAction;
            this.state1 = state1;
            this.state2 = state2;
            this.state3 = state3;
        }
        #endregion

        #region Methods
        protected override void OnComplete()
        {
            if (onAction != null)
            {
                onAction(state1, state2, state3);
                onAction = null;
            }
            state1 = default;
            state2 = default;
            state3 = default;
        }
        #endregion
    }
}
