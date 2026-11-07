using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Worker
{
    internal class WorkRegistry
    {
        #region properties
        private List<object> listActions;

        public bool IsEmpty => listActions.Count == 0;
        #endregion

        #region Constructions
        public WorkRegistry()
        {
            listActions = new();
        }
        #endregion

        #region Methods
        public void Clear()
        {
            listActions.Clear();
        }
        private int Get(out List<Action<WorkHandler>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action<WorkHandler>>)
                {
                    actions = listActions[i] as List<Action<WorkHandler>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T>(out List<Action<WorkHandler, T>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action<WorkHandler, T>>)
                {
                    actions = listActions[i] as List<Action<WorkHandler, T>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T1, T2>(out List<Action<WorkHandler, T1, T2>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action<WorkHandler, T1, T2>>)
                {
                    actions = listActions[i] as List<Action<WorkHandler, T1, T2>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T1, T2, T3>(out List<Action<WorkHandler, T1, T2, T3>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action<WorkHandler, T1, T2, T3>>)
                {
                    actions = listActions[i] as List<Action<WorkHandler, T1, T2, T3>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T1, T2, T3, T4>(out List<Action<WorkHandler, T1, T2, T3, T4>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action<WorkHandler, T1, T2, T3, T4>>)
                {
                    actions = listActions[i] as List<Action<WorkHandler, T1, T2, T3, T4>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        #endregion

        #region Register        
        public void Register(Action<WorkHandler> action)
        {
            int index = Get(out List<Action<WorkHandler>> _actions);
            if(index == -1)
            {
                _actions = new List<Action<WorkHandler>>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T>(Action<WorkHandler, T> action)
        {
            int index = Get(out List<Action<WorkHandler, T>> _actions);
            if(index == -1)
            {
                _actions = new List<Action<WorkHandler, T>>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T1, T2>(Action<WorkHandler, T1, T2> action)
        {
            int index = Get(out List<Action<WorkHandler, T1, T2>> _actions);
            if(index == -1)
            {
                _actions = new List<Action<WorkHandler, T1, T2>>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T1, T2, T3>(Action<WorkHandler, T1, T2, T3> action)
        {
            int index = Get(out List<Action<WorkHandler, T1, T2, T3>> _actions);
            if(index == -1)
            {
                _actions = new List<Action<WorkHandler, T1, T2, T3>>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T1, T2, T3, T4>(Action<WorkHandler, T1, T2, T3, T4> action)
        {
            int index = Get(out List<Action<WorkHandler, T1, T2, T3, T4>> _actions);
            if(index == -1)
            {
                _actions = new List<Action<WorkHandler, T1, T2, T3, T4>>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        #endregion

        #region Unregister    
        public void Unregister(Action<WorkHandler> action)
        {
            int index = Get(out List<Action<WorkHandler>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T>(Action<WorkHandler, T> action)
        {
            int index = Get(out List<Action<WorkHandler, T>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T1, T2>(Action<WorkHandler, T1, T2> action)
        {
            int index = Get(out List<Action<WorkHandler, T1, T2>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T1, T2, T3>(Action<WorkHandler, T1, T2, T3> action)
        {
            int index = Get(out List<Action<WorkHandler, T1, T2, T3>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T1, T2, T3, T4>(Action<WorkHandler, T1, T2, T3, T4> action)
        {
            int index = Get(out List<Action<WorkHandler, T1, T2, T3, T4>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        #endregion

        #region Check Register    
        public bool CheckRegister(Action<WorkHandler> action)
        {
            int index = Get(out List<Action<WorkHandler>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T>(Action<WorkHandler, T> action)
        {
            int index = Get(out List<Action<WorkHandler, T>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T1, T2>(Action<WorkHandler, T1, T2> action)
        {
            int index = Get(out List<Action<WorkHandler, T1, T2>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T1, T2, T3>(Action<WorkHandler, T1, T2, T3> action)
        {
            int index = Get(out List<Action<WorkHandler, T1, T2, T3>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T1, T2, T3, T4>(Action<WorkHandler, T1, T2, T3, T4> action)
        {
            int index = Get(out List<Action<WorkHandler, T1, T2, T3, T4>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        #endregion

        #region Notify
        public List<WorkHandler> Notify()
        {
            List<WorkHandler> listJobs = new();
            int index = Get(out List<Action<WorkHandler>> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                for(int i=0;i<tmp.Length;i++)
                {
                    WorkHandler job = new();
                    try{
                        tmp[i].Invoke(job);
                        listJobs.Add(job);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            return listJobs;
        }
        public List<WorkHandler> Notify<T>(T state)
        {
            List<WorkHandler> listJobs = new();
            int index = Get(out List<Action<WorkHandler, T>> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                for(int i=0;i<tmp.Length;i++)
                {
                    WorkHandler job = new();
                    try{
                        tmp[i].Invoke(job, state);
                        listJobs.Add(job);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            return listJobs;
        }
        public List<WorkHandler> Notify<T1, T2>(T1 state1, T2 state2)
        {
            List<WorkHandler> listJobs = new();
            int index = Get(out List<Action<WorkHandler, T1, T2>> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                for(int i=0;i<tmp.Length;i++)
                {
                    WorkHandler job = new();
                    try{
                        tmp[i].Invoke(job, state1, state2);
                        listJobs.Add(job);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            return listJobs;
        }
        public List<WorkHandler> Notify<T1, T2, T3>(T1 state1, T2 state2, T3 state3)
        {
            List<WorkHandler> listJobs = new();
            int index = Get(out List<Action<WorkHandler, T1, T2, T3>> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                for(int i=0;i<tmp.Length;i++)
                {
                    WorkHandler job = new();
                    try{
                        tmp[i].Invoke(job, state1, state2, state3);
                        listJobs.Add(job);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            return listJobs;
        }
        public List<WorkHandler> Notify<T1, T2, T3, T4>(T1 state1, T2 state2, T3 state3, T4 state4)
        {
            List<WorkHandler> listJobs = new();
            int index = Get(out List<Action<WorkHandler, T1, T2, T3, T4>> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                for(int i=0;i<tmp.Length;i++)
                {
                    WorkHandler job = new();
                    try{
                        tmp[i].Invoke(job, state1, state2, state3, state4);
                        listJobs.Add(job);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            return listJobs;
        }
        #endregion
    }
}
