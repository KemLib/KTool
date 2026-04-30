using UnityEngine;

namespace KTool.Cron
{
    public class CallbackAction : Callback
    {
        #region Properties
        public delegate void ActionDelegate();
        private ActionDelegate onAction;
        #endregion

        #region Construction
        internal CallbackAction(ActionDelegate onAction) : base()
        {
            this.onAction = onAction;
        }
        #endregion

        #region Methods
        protected override void OnComplete()
        {
            if (onAction != null)
            {
                onAction();
                onAction = null;
            }
        }
        #endregion

        #region Build
        public static Callback Create(ActionDelegate onAction)
        {
            return new CallbackAction(onAction);
        }
        public static Callback Create<T>(CallbackAction<T>.ActionDelegate onAction, T state)
        {
            return new CallbackAction<T>(onAction, state);
        }
        public static Callback Create<T1, T2>(CallbackAction<T1, T2>.ActionDelegate onAction, T1 state1, T2 state2)
        {
            return new CallbackAction<T1, T2>(onAction, state1, state2);
        }
        public static Callback Create<T1, T2, T3>(CallbackAction<T1, T2, T3>.ActionDelegate onAction, T1 state1, T2 state2, T3 state3)
        {
            return new CallbackAction<T1, T2, T3>(onAction, state1, state2, state3);
        }
        public static Callback Create<T1, T2, T3, T4>(CallbackAction<T1, T2, T3, T4>.ActionDelegate onAction, T1 state1, T2 state2, T3 state3, T4 state4)
        {
            return new CallbackAction<T1, T2, T3, T4>(onAction, state1, state2, state3, state4);
        }
        #endregion
    }
}
