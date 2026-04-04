using UnityEngine;

namespace KTool.Cron
{
    public class CallbackAction<T> : Callback
    {
        #region Properties
        public delegate void ActionDelegate(T state);
        private ActionDelegate onAction;
        private T state;
        #endregion

        #region Construction
        internal CallbackAction(ActionDelegate onAction, T state) : base()
        {
            this.onAction = onAction;
            this.state = state;
        }
        #endregion

        #region Methods
        protected override void OnComplete()
        {
            if (onAction != null)
            {
                onAction(state);
                onAction = null;
            }
            state = default(T);
        }
        #endregion

        #region Build
        public static Callback Create(ActionDelegate onAction, T state)
        {
            return new CallbackAction<T>(onAction, state);
        }
        #endregion
    }
}
