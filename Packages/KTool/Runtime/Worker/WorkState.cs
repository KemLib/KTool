using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Worker
{
    internal class WorkState
    {
        #region Properties
        private readonly List<WorkHandler> listHandlers;
        private readonly Action<List<WorkHandler>> callback;
        private readonly float timeout;
        private readonly int frameLimit;
        private readonly float startTime;
        private int frameCount;
        #endregion

        #region Construction
        public WorkState(List<WorkHandler> listHandlers, Action<List<WorkHandler>> callback, float timeout = -1, int frameLimit = -1)
        {
            this.listHandlers = listHandlers;
            this.callback = callback;
            this.timeout = timeout;
            this.frameLimit = frameLimit;
            this.startTime = Time.time;
            this.frameCount = 0;
        }
        #endregion

        #region Methods
        public bool CheckAllDone()
        {
            // check timeout
            if(CheckTimeout() || CheckFrameLimit())
            {
                // timeout or frame limit
                foreach(var handler in listHandlers)
                {
                    handler.Timeout();
                }
            }
            else
            {
                // check all done
                foreach(var handler in listHandlers)
                {
                    if(!handler.IsDone)
                        return false;
                }
            }
            // callback
            try{
                callback?.Invoke(listHandlers);
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
            return timeout > 0 && Time.time - startTime > timeout;
        }
        private bool CheckFrameLimit()
        {
            frameCount++;
            return frameLimit > 0 && frameCount > frameLimit;
        }
        #endregion
    }
}
