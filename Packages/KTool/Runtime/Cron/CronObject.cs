using System;
using System.Collections.Generic;
using UnityEngine;
using KLibStandard.Concurrent;

namespace KTool.Cron
{
    public class CronObject
    {
        #region Properties
        private InterValueBool isComplete;
        private List<Condition> conditions;
        private List<Callback> callbacks;

        public bool IsComplete => isComplete;
        #endregion

        #region Contructors
        internal CronObject()
        {
            isComplete = new (false);
            conditions = new List<Condition>();
            callbacks = new List<Callback>();
        }
        #endregion

        #region Methods
        public void Cancel()
        {
            if(!isComplete.TryExchange(true))
                return;
            //
            conditions.Clear();
            conditions = null;
            //
            callbacks.Clear();
            callbacks = null;
        }
        internal void Update()
        {
            if (isComplete.Value)
                return;
            //
            if (UpdateCondition())
                UpdateCallback();
        }
        private bool UpdateCondition()
        {
            if(conditions == null)
                return false;
            //
            int index = 0;
            while(index < conditions.Count)
            {
                conditions[index].Check();
                if(isComplete.Value)
                    return false;
                if(conditions[index].IsComplete)
                    conditions.RemoveAt(index);
                else
                    index++;
            }
            //
            if(conditions.Count == 0 && isComplete.TryExchange(true))
            {
                conditions = null;
                return true;
            }
            //
            return false;
        }
        private void UpdateCallback()
        {
            if(callbacks == null)
                return;
            //
            foreach (var callback in callbacks)
            {
                try{
                    callback.SetComplete();
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
            }
            //
            callbacks = null;
        }
        #endregion

        #region Build
        public static CronObject Create()
        {
            return new CronObject();
        }
        public CronObject Add(Condition condition)
        {
            if (isComplete.Value)
                return this;
            //
            if(condition == null || condition.IsComplete)
                return this;
            //
            if(conditions != null && !conditions.Contains(condition))
            {
                conditions.Add(condition);
            }
            return this;
        }
        public CronObject Add(Callback callback)
        {
            if (isComplete.Value)
                return this;
            //
            if(callback == null || callback.IsComplete)
                return this;
            //
            if(callbacks!= null && !callbacks.Contains(callback))
            {
                callbacks.Add(callback);
            }
            return this;
        }
        public void Run()
        {
            if (isComplete.Value)
                return;
            //
            CronManager.Instance.Cron_Add(this);
        }
        #endregion
    }
}
