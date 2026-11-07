using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.Observer
{
    public class ObserverManager : MonoBehaviour
    {
        #region Properties
        private static readonly string GAME_OBJECT_NAME = "ObserverManager";
        private static ObserverManager instance;
        public static ObserverManager Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject newGO = new(GAME_OBJECT_NAME);
                    instance = newGO.AddComponent<ObserverManager>();
                    DontDestroyOnLoad(newGO);                
                }
                return instance;
            }
        }

        private Dictionary<string, Registry> dicRegistries = new ();
        #endregion

        #region Methods Unity
        private void OnDestroy()
        {
            if(instance.GetEntityId() == this.GetEntityId())
            {
                foreach(var registry in dicRegistries.Values)
                    registry.Clear();
                dicRegistries.Clear();
                instance = null;
            }
        }
        #endregion

        #region Register
        public void Register(string key, UnityAction action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                Registry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);

            }
        }
        public void Register<T>(string key, UnityAction<T> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                Registry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);
            }
        }
        public void Register<T1, T2>(string key, UnityAction<T1, T2> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                Registry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);
            }
        }
        public void Register<T1, T2, T3>(string key, UnityAction<T1, T2, T3> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                Registry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);
            }
        }
        public void Register<T1, T2, T3, T4>(string key, UnityAction<T1, T2, T3, T4> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Register(action);
            }
            else
            {
                Registry registry = new();
                registry.Register(action);
                dicRegistries.Add(key, registry);
            }
        }
        #endregion

        #region Unregister
        public void Unregister(string key, UnityAction action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
            }
        }
        public void Unregister<T>(string key, UnityAction<T> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
            }
        }
        public void Unregister<T1, T2>(string key, UnityAction<T1, T2> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
            }
        }
        public void Unregister<T1, T2, T3>(string key, UnityAction<T1, T2, T3> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
            }
        }
        public void Unregister<T1, T2, T3, T4>(string key, UnityAction<T1, T2, T3, T4> action)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Unregister(action);
            }
        }
        #endregion

        #region Check Register
        public bool CheckRegister(string key, UnityAction action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        public bool CheckRegister<T>(string key, UnityAction<T> action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        public bool CheckRegister<T1, T2>(string key, UnityAction<T1, T2> action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        public bool CheckRegister<T1, T2, T3>(string key, UnityAction<T1, T2, T3> action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        public bool CheckRegister<T1, T2, T3, T4>(string key, UnityAction<T1, T2, T3, T4> action)
        {
            if(dicRegistries.ContainsKey(key))
                return dicRegistries[key].CheckRegister(action);
            return false;
        }
        #endregion

        #region Notify
        public void Notify(string key)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Notify();
            }
        }
        public void Notify<T>(string key, T state)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Notify(state);
            }
        }
        public void Notify<T1, T2>(string key, T1 state1, T2 state2)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Notify(state1, state2);
            }
        }
        public void Notify<T1, T2, T3>(string key, T1 state1, T2 state2, T3 state3)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Notify(state1, state2, state3);
            }
        }
        public void Notify<T1, T2, T3, T4>(string key, T1 state1, T2 state2, T3 state3, T4 state4)
        {
            if(dicRegistries.ContainsKey(key))
            {
                dicRegistries[key].Notify(state1, state2, state3, state4);
            }
        }
        #endregion
    }
}
