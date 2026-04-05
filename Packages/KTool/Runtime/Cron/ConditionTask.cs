using UnityEngine;
using System.Threading.Tasks;

namespace KTool.Cron
{
    public class ConditionTask : Condition
    {
        #region Properties
        public delegate bool CheckConditionDelegate();
        private Task task;
        #endregion

        #region Construction
        internal ConditionTask(Task task) : base()
        {
            this.task = task;
        }
        #endregion

        #region Methods
        protected override void OnCheck()
        {
            if (task == null)
            {
                SetComplete();
                return;
            }
            //
            if (task.IsCompleted)
            {
                SetComplete();
                task = null;
            }
        }
        #endregion

        #region Build
        public static Condition Create(Task task)
        {
            return new ConditionTask(task);
        }
        #endregion
    }
}
