using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.Observer
{
    public class Registry
    {
        #region properties
        private List<object> listActions;
        #endregion

        #region Constructions
        public Registry()
        {
            listActions = new ();
        }
        #endregion

        #region Methods
        public void Clear()
        {
            listActions.Clear();
        }
        private int Get(out List<UnityAction> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<UnityAction>)
                {
                    actions = listActions[i] as List<UnityAction>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T>(out List<UnityAction<T>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<UnityAction<T>>)
                {
                    actions = listActions[i] as List<UnityAction<T>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T1, T2>(out List<UnityAction<T1, T2>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<UnityAction<T1, T2>>)
                {
                    actions = listActions[i] as List<UnityAction<T1, T2>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T1, T2, T3>(out List<UnityAction<T1, T2, T3>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<UnityAction<T1, T2, T3>>)
                {
                    actions = listActions[i] as List<UnityAction<T1, T2, T3>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        private int Get<T1, T2, T3, T4>(out List<UnityAction<T1, T2, T3, T4>> actions)
        {
            for(int i=0;i<listActions.Count;i++)
                if(listActions[i] is List<UnityAction<T1, T2, T3, T4>>)
                {
                    actions = listActions[i] as List<UnityAction<T1, T2, T3, T4>>;
                    return i;
                }
            actions = null;
            return -1;
        }
        #endregion

        #region Register        
        public void Register(UnityAction action)
        {
            int index = Get(out List<UnityAction> _actions);
            if(index == -1)
            {
                _actions = new List<UnityAction>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T>(UnityAction<T> action)
        {
            int index = Get(out List<UnityAction<T>> _actions);
            if(index == -1)
            {
                _actions = new List<UnityAction<T>>();
                listActions.Add(_actions);
                _actions.Add(action);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T1, T2>(UnityAction<T1, T2> action)
        {
            int index = Get(out List<UnityAction<T1, T2>> _actions);
            if(index == -1)
            {
                _actions = new List<UnityAction<T1, T2>>();
                listActions.Add(_actions);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T1, T2, T3>(UnityAction<T1, T2, T3> action)
        {
            int index = Get(out List<UnityAction<T1, T2, T3>> _actions);
            if(index == -1)
            {
                _actions = new List<UnityAction<T1, T2, T3>>();
                listActions.Add(_actions);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        public void Register<T1, T2, T3, T4>(UnityAction<T1, T2, T3, T4> action)
        {
            int index = Get(out List<UnityAction<T1, T2, T3, T4>> _actions);
            if(index == -1)
            {
                _actions = new List<UnityAction<T1, T2, T3, T4>>();
                listActions.Add(_actions);
            }
            else if(!_actions.Contains(action))
            {
                _actions.Add(action);
            }
        }
        #endregion

        #region Unregister    
        public void Unregister(UnityAction action)
        {
            int index = Get(out List<UnityAction> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T>(UnityAction<T> action)
        {
            int index = Get(out List<UnityAction<T>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T1, T2>(UnityAction<T1, T2> action)
        {
            int index = Get(out List<UnityAction<T1, T2>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T1, T2, T3>(UnityAction<T1, T2, T3> action)
        {
            int index = Get(out List<UnityAction<T1, T2, T3>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        public void Unregister<T1, T2, T3, T4>(UnityAction<T1, T2, T3, T4> action)
        {
            int index = Get(out List<UnityAction<T1, T2, T3, T4>> _actions);
            if(index != -1 && _actions.Contains(action))
            {
                _actions.Remove(action);
                if(_actions.Count == 0)
                    listActions.RemoveAt(index);
            }
        }
        #endregion

        #region Check Register    
        public bool CheckRegister(UnityAction action)
        {
            int index = Get(out List<UnityAction> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T>(UnityAction<T> action)
        {
            int index = Get(out List<UnityAction<T>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T1, T2>(UnityAction<T1, T2> action)
        {
            int index = Get(out List<UnityAction<T1, T2>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T1, T2, T3>(UnityAction<T1, T2, T3> action)
        {
            int index = Get(out List<UnityAction<T1, T2, T3>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        public bool CheckRegister<T1, T2, T3, T4>(UnityAction<T1, T2, T3, T4> action)
        {
            int index = Get(out List<UnityAction<T1, T2, T3, T4>> _actions);
            return index != -1 && _actions.Contains(action);
        }
        #endregion

        #region Notify
        public void Notify()
        {
            int index = Get(out List<UnityAction> _actions);
            if(index != -1 && _actions.Count > 0)
                foreach(var action in _actions)
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
        public void Notify<T>(T state)
        {
            int index = Get(out List<UnityAction<T>> _actions);
            if(index != -1 && _actions.Count > 0)
                foreach(var action in _actions)
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
        public void Notify<T1, T2>(T1 state1, T2 state2)
        {
            int index = Get(out List<UnityAction<T1, T2>> _actions);
            if(index != -1 && _actions.Count > 0)
                foreach(var action in _actions)
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
        public void Notify<T1, T2, T3>(T1 state1, T2 state2, T3 state3)
        {
            int index = Get(out List<UnityAction<T1, T2, T3>> _actions);
            if(index != -1 && _actions.Count > 0)
                foreach(var action in _actions)
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
        public void Notify<T1, T2, T3, T4>(T1 state1, T2 state2, T3 state3, T4 state4)
        {
            int index = Get(out List<UnityAction<T1, T2, T3, T4>> _actions);
            if(index != -1 && _actions.Count > 0)
                foreach(var action in _actions)
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
        #endregion
    }
}
