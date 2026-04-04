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
        #endregion
    }
}
