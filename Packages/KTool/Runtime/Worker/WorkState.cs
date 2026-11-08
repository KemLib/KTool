using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Worker
{
    public class WorkState
    {
        #region Properties
        public readonly List<WorkHandler> ListHandlers;
        private readonly Action<List<WorkHandler>> callback;
        public readonly float Timeout;
        public readonly int FrameLimit;
        public readonly float StartTime;
        private int frameCount;
        private bool isDone;
        
        public int FrameCount => frameCount;
        public bool IsDone => isDone;
        #endregion

        #region Construction
        internal WorkState(List<WorkHandler> listHandlers, Action<List<WorkHandler>> callback, float timeout = -1, int frameLimit = -1)
        {
            this.ListHandlers = listHandlers;
            this.callback = callback;
            this.Timeout = timeout;
            this.FrameLimit = frameLimit;
            this.StartTime = Time.time;
            this.frameCount = 0;
            this.isDone = false;
        }
        #endregion

        #region Methods
        internal bool CheckAllDone()
        {
            if(isDone)
                return true;
            // check timeout
            if(CheckTimeout() || CheckFrameLimit())
            {
                // timeout or frame limit
                foreach(var handler in ListHandlers)
                {
                    handler.Timeout();
                }
            }
            else
            {
                // check all done
                foreach(var handler in ListHandlers)
                {
                    if(!handler.IsDone)
                        return false;
                }
            }
            // callback
            isDone = true;
            try{
                callback?.Invoke(ListHandlers);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
            // return true
            return true;
        }
        private bool CheckTimeout()
        {
            return Timeout > 0 && Time.time - StartTime > Timeout;
        }
        private bool CheckFrameLimit()
        {
            frameCount++;
            return FrameLimit > 0 && frameCount > FrameLimit;
        }
        #endregion
    }
}
