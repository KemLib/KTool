using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.MultiThread
{
    public class ThreadManager : MonoBehaviour
    {
        #region Properties
        private const string GAME_OBJECT_NAME = "Thread Manager";

        private static ThreadManager instance;
        public static ThreadManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject newObject = new(GAME_OBJECT_NAME);
                    DontDestroyOnLoad(newObject);
                    instance = newObject.AddComponent<ThreadManager>();
                    instance.Init();
                }
                return instance;
            }
        }

        private List<KTask> tasks;
        #endregion

        #region UnityEvent
        private void Update()
        {
            Update_Task();
        }
        #endregion

        #region Method
        private void Init()
        {
            tasks = new List<KTask>();
        }
        private void Update_Task()
        {
            int index = 0;
            while (index < tasks.Count)
            {
                tasks[index].Update();
                if (tasks[index].IsComplete)
                {
                    tasks.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }
        #endregion

        #region Task
        public IKTask Add(Action func, object state = null, UnityAction<Result> onComplete = null)
        {
            KTask task = new KTaskFunc(func, state, onComplete);
            return Add(task);
        }
        public IKTask Add<T>(Func<T> func, object state = null, UnityAction<Result<T>> onComplete = null)
        {
            KTask task = new KTaskFunc<T>(func, state, onComplete);
            return Add(task);
        }
        public IKTask Add(KTask task)
        {
            task.Start();
            if (!task.IsComplete)
                tasks.Add(task);
            return task;
        }
        #endregion
    }
}
