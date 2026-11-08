using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Worker
{
    public class WorkManager : MonoBehaviour
    {
        #region Properties
        private static readonly string GAME_OBJECT_NAME = "KTool_WorkManager";
        private static WorkManager instance;
        public static WorkManager Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject newGO = new(GAME_OBJECT_NAME);
                    newGO.AddComponent<WorkManager>();
                }
                return instance;
            }
        }
        private Dictionary<string, WorkRegistry> dicRegistries = new ();
        private List<WorkState> listStates = new();
        #endregion

        #region Methods Unity        
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        private void OnDestroy()
        {
            if(instance != null && instance.GetEntityId() == this.GetEntityId())
            {
                foreach(var registry in dicRegistries.Values)
                    registry.Clear();
                dicRegistries.Clear();
                listStates.Clear();
                instance = null;
            }
        }
        private void Update()
        {
            int index = 0;
            while(index < listStates.Count)
            {
                WorkState state = listStates[index];
                if(state.CheckAllDone())
                {
                    listStates.RemoveAt(index);
                    continue;
                }
                index++;
            }
        }
        #endregion

        #region Methods

        #endregion

        #region Register
        public void Register(string key, Action<WorkHandler> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                WorkRegistry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);

            }
        }
        public void Register<T>(string key, Action<WorkHandler, T> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                WorkRegistry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);
            }
        }
        public void Register<T1, T2>(string key, Action<WorkHandler, T1, T2> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                WorkRegistry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);
            }
        }
        public void Register<T1, T2, T3>(string key, Action<WorkHandler, T1, T2, T3> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                WorkRegistry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);
            }
        }
        public void Register<T1, T2, T3, T4>(string key, Action<WorkHandler, T1, T2, T3, T4> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                WorkRegistry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);
            }
        }
        #endregion

        #region Unregister
        public void Unregister(string key, Action<WorkHandler> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
                if(dicRegistries[key].IsEmpty)
                    dicRegistries.Remove(key);
            }
        }
        public void Unregister<T>(string key, Action<WorkHandler, T> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
                if(dicRegistries[key].IsEmpty)
                    dicRegistries.Remove(key);
            }
        }
        public void Unregister<T1, T2>(string key, Action<WorkHandler, T1, T2> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
                if(dicRegistries[key].IsEmpty)
                    dicRegistries.Remove(key);
            }
        }
        public void Unregister<T1, T2, T3>(string key, Action<WorkHandler, T1, T2, T3> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
                if(dicRegistries[key].IsEmpty)
                    dicRegistries.Remove(key);
            }
        }
        public void Unregister<T1, T2, T3, T4>(string key, Action<WorkHandler, T1, T2, T3, T4> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
                if(dicRegistries[key].IsEmpty)
                    dicRegistries.Remove(key);
            }
        }
        #endregion

        #region Check Register
        public bool CheckRegister(string key, Action<WorkHandler> action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        public bool CheckRegister<T>(string key, Action<WorkHandler, T> action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        public bool CheckRegister<T1, T2>(string key, Action<WorkHandler, T1, T2> action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        public bool CheckRegister<T1, T2, T3>(string key, Action<WorkHandler, T1, T2, T3> action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        public bool CheckRegister<T1, T2, T3, T4>(string key, Action<WorkHandler, T1, T2, T3, T4> action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        #endregion

        #region Notify
        public WorkState Notify(string key, Action<List<WorkHandler>> callback, float timeout = -1, int frameLimit = -1)
        {
            List<WorkHandler> listHandlers;
            if(dicRegistries.ContainsKey(key))
                 listHandlers = dicRegistries[key].Notify();
            else
                listHandlers = new();

            WorkState workState = new(listHandlers, callback, timeout, frameLimit);
            listStates.Add(workState);
            return workState;
        }
        public WorkState Notify<T>(string key, T state, Action<List<WorkHandler>> callback, float timeout = -1, int frameLimit = -1)
        {
            List<WorkHandler> listHandlers;
            if(dicRegistries.ContainsKey(key))
                listHandlers = dicRegistries[key].Notify(state);
            else
                listHandlers = new();

            WorkState workState = new(listHandlers, callback, timeout, frameLimit);
            listStates.Add(workState);
            return workState;
        }
        public WorkState Notify<T1, T2>(string key, T1 state1, T2 state2, Action<List<WorkHandler>> callback, float timeout = -1, int frameLimit = -1)
        {
            List<WorkHandler> listHandlers;
            if(dicRegistries.ContainsKey(key))
                listHandlers = dicRegistries[key].Notify(state1, state2);
            else
                listHandlers = new();

            WorkState workState = new(listHandlers, callback, timeout, frameLimit);
            listStates.Add(workState);
            return workState;
        }
        public WorkState Notify<T1, T2, T3>(string key, T1 state1, T2 state2, T3 state3, Action<List<WorkHandler>> callback, float timeout = -1, int frameLimit = -1)
        {
            List<WorkHandler> listHandlers;
            if(dicRegistries.ContainsKey(key))
                listHandlers = dicRegistries[key].Notify(state1, state2, state3);
            else
                listHandlers = new();

            WorkState workState = new(listHandlers, callback, timeout, frameLimit);
            listStates.Add(workState);
            return workState;
        }
        public WorkState Notify<T1, T2, T3, T4>(string key, T1 state1, T2 state2, T3 state3, T4 state4, Action<List<WorkHandler>> callback, float timeout = -1, int frameLimit = -1)
        {
            List<WorkHandler> listHandlers;
            if(dicRegistries.ContainsKey(key))
                listHandlers = dicRegistries[key].Notify(state1, state2, state3, state4);
            else
                listHandlers = new();

            WorkState workState = new(listHandlers, callback, timeout, frameLimit);
            listStates.Add(workState);
            return workState;
        }
        #endregion
    }
}
