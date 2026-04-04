using System.Collections.Generic;

namespace KTool.Cron
{
    public class CronObject
    {
        #region Properties
        private bool isComplete;
        private List<Condition> conditions;
        private List<Callback> callbacks;

        public bool IsComplete => isComplete;
        #endregion

        #region Contructors
        internal CronObject()
        {
            isComplete = false;
            conditions = new List<Condition>();
            callbacks = new List<Callback>();
        }
        #endregion

        #region Methods
        public void Cancel()
        {
            isComplete = true;
            conditions = null;
            callbacks = null;
        }
        internal void Update()
        {
            if (isComplete)
                return;
            //
            int index = 0;
            while (index < conditions.Count)
            {
                conditions[index].Check();
                if (conditions[index].IsComplete)
                    conditions.RemoveAt(index);
                else
                    index++;
            }
            if (conditions.Count <= 0)
            {
                isComplete = true;
                foreach (var callback in callbacks)
                    callback.SetComplete();
                //
                conditions = null;
                callbacks = null;
            }
        }
        #endregion

        #region Build
        public static CronObject Create()
        {
            return new CronObject();
        }
        public CronObject Add(Condition condition)
        {
            if (isComplete)
                return this;
            //
            conditions.Add(condition);
            return this;
        }
        public CronObject Add(Callback callback)
        {
            if (isComplete)
                return this;
            //
            callbacks.Add(callback);
            return this;
        }
        public void Run()
        {
            if (isComplete)
                return;
            //
            CronManager.Instance.Cron_Add(this);
        }
        #endregion
    }
}
