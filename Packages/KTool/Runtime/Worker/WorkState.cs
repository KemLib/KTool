using System;
using System.Collections.Generic;
using UnityEngine;
using KLibStandard.Concurrent;

namespace KTool.Worker
{
    public class WorkState
    {
        #region Properties
        public readonly IReadOnlyList<WorkHandler> ListWorker;
        private readonly Action<WorkState> callback;
        public readonly float Timeout;
        public readonly int FrameLimit;
        public readonly float StartTime;
        private int frameCount;
        private InterValueInt _state;
        private InterValueFloat _progress;
        
        public int FrameCount => frameCount;
        public float Progress => _progress.Value;
        public bool IsDone => _state.Value != WorkHandler.STATE_PENDING;
        public bool IsSuccess => _state.Value == WorkHandler.STATE_SUCCESS;
        public bool IsCanceled => _state.Value == WorkHandler.STATE_CANCELED;
        public bool IsFailed => _state.Value == WorkHandler.STATE_FAILED;
        public bool IsTimeout => _state.Value == WorkHandler.STATE_TIMEOUT;
        #endregion

        #region Construction
        internal WorkState(List<WorkHandler> workers, Action<WorkState> callback, float timeout = -1, int frameLimit = -1)
        {
            ListWorker = workers;
            this.callback = callback;
            this.Timeout = timeout;
            this.FrameLimit = frameLimit;
            this.StartTime = Time.time;
            this.frameCount = 0;
            _state = new(WorkHandler.STATE_PENDING);
            _progress = new(0);
        }
        #endregion

        #region Methods        
        internal bool CheckAllDone()
        {
            if(IsDone)
                return true;
            //
            frameCount++;
            if(Timeout > 0 && Time.time - StartTime > Timeout)
            {
                SetTimeout();
                return true;
            }
            
            bool isAllDone = true,
                isAllSuccess = true;
            float totalProgress = 0f;
            foreach(var handler in ListWorker)
            {
                totalProgress += handler.Progress;
                if(handler.IsDone)
                {
                    if(!handler.IsSuccess)
                        isAllSuccess = false;
                }
                else
                {
                    isAllDone = false;
                }
            }
            if(ListWorker.Count <=0 )
                _progress.Value = 1;
            else
                _progress.Value = totalProgress / ListWorker.Count;
            //
            if(isAllDone)
            {
                if(isAllSuccess)
                {
                    SetSuccess();
                    return true;
                }
                else
                {
                    SetFailed();
                    return true;
                }
            }
            else
            {
                if(FrameLimit > 0 && frameCount >= FrameLimit)
                {
                    SetTimeout();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private void SetSuccess()
        {
            if(!_state.TryCompareExchange(WorkHandler.STATE_SUCCESS, WorkHandler.STATE_PENDING))
                return;
            //
            try{
                callback?.Invoke(this);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }
        private void SetFailed()
        {
            if(!_state.TryCompareExchange(WorkHandler.STATE_FAILED, WorkHandler.STATE_PENDING))
                return;
            //
            try{
                callback?.Invoke(this);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }
        private void SetTimeout()
        {
            if(!_state.TryCompareExchange(WorkHandler.STATE_TIMEOUT, WorkHandler.STATE_PENDING))
                return;
            //
            foreach(var handler in ListWorker)
                handler.Timeout();
            try{
                callback?.Invoke(this);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }
        public void SetCancel()
        {
            if(!_state.TryCompareExchange(WorkHandler.STATE_CANCELED, WorkHandler.STATE_PENDING))
                return;
            //
            foreach(var handler in ListWorker)
                handler.Cancel();
            try{
                callback?.Invoke(this);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }
        #endregion
    }
}
