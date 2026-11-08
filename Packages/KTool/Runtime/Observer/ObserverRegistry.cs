using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Observer
{
    internal class ObserverRegistry
    {
        #region properties
        private List<object> listActions;

        public bool IsEmpty => listActions.Count == 0;
        #endregion

        #region Constructions
        public ObserverRegistry()
        {
            listActions = new ();
        }
        #endregion

        #region Methods
        public void Clear()
        {
            listActions.Clear();
        }
        private int Get(out List<Action> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action>)
                {
                    actions = listActions[i] as List<Action>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T>(out List<Action<T>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action<T>>)
                {
                    actions = listActions[i] as List<Action<T>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T1, T2>(out List<Action<T1, T2>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action<T1, T2>>)
                {
                    actions = listActions[i] as List<Action<T1, T2>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T1, T2, T3>(out List<Action<T1, T2, T3>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action<T1, T2, T3>>)
                {
                    actions = listActions[i] as List<Action<T1, T2, T3>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T1, T2, T3, T4>(out List<Action<T1, T2, T3, T4>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<Action<T1, T2, T3, T4>>)
                {
                    actions = listActions[i] as List<Action<T1, T2, T3, T4>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        #endregion

        #region Register        
        public void Register(Action action)
        {
            int index = Get(out List<Action> _actions);
            if(index == -1)
            {
                _actions = new List<Action>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T>(Action<T> action)
        {
            int index = Get(out List<Action<T>> _actions);
            if(index == -1)
            {
                _actions = new List<Action<T>>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T1, T2>(Action<T1, T2> action)
        {
            int index = Get(out List<Action<T1, T2>> _actions);
            if(index == -1)
            {
                _actions = new List<Action<T1, T2>>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            int index = Get(out List<Action<T1, T2, T3>> _actions);
            if(index == -1)
            {
                _actions = new List<Action<T1, T2, T3>>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            int index = Get(out List<Action<T1, T2, T3, T4>> _actions);
            if(index == -1)
            {
                _actions = new List<Action<T1, T2, T3, T4>>();
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
        public void Unregister(Action action)
        {
            int index = Get(out List<Action> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T>(Action<T> action)
        {
            int index = Get(out List<Action<T>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T1, T2>(Action<T1, T2> action)
        {
            int index = Get(out List<Action<T1, T2>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            int index = Get(out List<Action<T1, T2, T3>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            int index = Get(out List<Action<T1, T2, T3, T4>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        #endregion

        #region Check Register    
        public bool CheckRegister(Action action)
        {
            int index = Get(out List<Action> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T>(Action<T> action)
        {
            int index = Get(out List<Action<T>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T1, T2>(Action<T1, T2> action)
        {
            int index = Get(out List<Action<T1, T2>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            int index = Get(out List<Action<T1, T2, T3>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            int index = Get(out List<Action<T1, T2, T3, T4>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        #endregion

        #region Notify
        public void Notify()
        {
            int index = Get(out List<Action> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                foreach(var action in tmp)
                {
                    try{
                        action.Invoke();
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
        public void Notify<T>(T state)
        {
            int index = Get(out List<Action<T>> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                foreach(var action in tmp)
                {
                    try{
                        action.Invoke(state);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
        public void Notify<T1, T2>(T1 state1, T2 state2)
        {
            int index = Get(out List<Action<T1, T2>> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                foreach(var action in tmp)
                {
                    try{
                        action.Invoke(state1, state2);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
        public void Notify<T1, T2, T3>(T1 state1, T2 state2, T3 state3)
        {
            int index = Get(out List<Action<T1, T2, T3>> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                foreach(var action in tmp)
                {
                    try{
                        action.Invoke(state1, state2, state3);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
        public void Notify<T1, T2, T3, T4>(T1 state1, T2 state2, T3 state3, T4 state4)
        {
            int index = Get(out List<Action<T1, T2, T3, T4>> _actions);
            if(index != -1 && _actions.Count > 0)
            {
                var tmp = _actions.ToArray();
                foreach(var action in tmp)
                {
                    try{
                        action.Invoke(state1, state2, state3, state4);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
        #endregion
    }
}
