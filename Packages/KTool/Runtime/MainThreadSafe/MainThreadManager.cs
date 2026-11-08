using UnityEngine;
using System;
using KLibStandard.Concurrent.Queue;

namespace KTool.MainThreadSafe
{
    public class MainThreadManager : MonoBehaviour
    {
        #region Properties
        private static readonly string GAME_OBJECT_NAME = "KTool_MainThreadManager";
        private static MainThreadManager instance;
        private static TwiceQueue<IAction> queueActions = new();
        private static IQueueIn<IAction> defaultQueueIn = queueActions.CreateQueueIn();
        #endregion

        #region Methods Unity
        private void Awake()
        {
            if(instance != null)
            {
                Destroy(this.gameObject);
            }
        }
        private void OnDestroy()
        {
            if(instance != null && instance.GetEntityId() == this.GetEntityId())
            {
                instance = null;
            }
        }
        private void Update()
        {
            if(queueActions.TryDequeueAll(out var actions))
            {
                foreach(var action in actions)
                {
                    try
                    {
                        action.Execute();
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
        #endregion

        #region Methods
        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            if(instance != null)
                return;
            GameObject newGO = new(GAME_OBJECT_NAME);
            DontDestroyOnLoad(newGO);
            instance = newGO.AddComponent<MainThreadManager>();
        }

        public static IQueueIn<IAction> GetNewQueue()
        {
            return queueActions.CreateQueueIn();
        }
        #endregion        

        #region Actions
        public static void Add(Action action)
        {
            defaultQueueIn.Enqueue(new ActionT0(action));
        }
        public static void Add(Action action, IQueueIn<IAction> queue)
        {
            queue?.Enqueue(new ActionT0(action));
        }
        public static void Add<T>(Action<T> action, T state)
        {
            defaultQueueIn.Enqueue(new ActionT1<T>(action, state));
        }
        public static void Add<T>(Action<T> action, T state, IQueueIn<IAction> queue)
        {
            queue?.Enqueue(new ActionT1<T>(action, state));
        }  
        public static void Add<T1, T2>(Action<T1, T2> action, T1 state1, T2 state2)
        {
            defaultQueueIn.Enqueue(new ActionT2<T1, T2>(action, state1, state2));
        }
        public static void Add<T1, T2>(Action<T1, T2> action, T1 state1, T2 state2, IQueueIn<IAction> queue)
        {
            queue?.Enqueue(new ActionT2<T1, T2>(action, state1, state2));
        }   
        public static void Add<T1, T2, T3>(Action<T1, T2, T3> action, T1 state1, T2 state2, T3 state3)
        {
            defaultQueueIn.Enqueue(new ActionT3<T1, T2, T3>(action, state1, state2, state3));
        }
        public static void Add<T1, T2, T3>(Action<T1, T2, T3> action, T1 state1, T2 state2, T3 state3, IQueueIn<IAction> queue)
        {
            queue?.Enqueue(new ActionT3<T1, T2, T3>(action, state1, state2, state3));
        }
        public static void Add<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 state1, T2 state2, T3 state3, T4 state4)
        {
            defaultQueueIn.Enqueue(new ActionT4<T1, T2, T3, T4>(action, state1, state2, state3, state4));
        }
        public static void Add<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 state1, T2 state2, T3 state3, T4 state4, IQueueIn<IAction> queue)
        {
            queue?.Enqueue(new ActionT4<T1, T2, T3, T4>(action, state1, state2, state3, state4));
        }
        #endregion


    }
}
