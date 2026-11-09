using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Worker
{
    public class WorkState
    {
        #region Properties
        private readonly List<WorkHandler> listWorkerHandler;
        private readonly Action<IReadOnlyList<IWorker>> callback;
        public readonly float Timeout;
        public readonly int FrameLimit;
        public readonly float StartTime;
        public readonly IReadOnlyList<IWorker> ListWorker;
        private int frameCount;
        private bool isDone;
        
        public int FrameCount => frameCount;
        public bool IsDone => isDone;
        #endregion

        #region Construction
        internal WorkState(List<WorkHandler> workers, Action<IReadOnlyList<IWorker>> callback, float timeout = -1, int frameLimit = -1)
        {
            this.listWorkerHandler = workers;
            this.callback = callback;
            this.Timeout = timeout;
            this.FrameLimit = frameLimit;
            this.StartTime = Time.time;
            this.frameCount = 0;
            this.isDone = false;
            List<IWorker> tmp = new (listWorkerHandler.Count);
            foreach(var handler in listWorkerHandler)
                tmp.Add(handler);
            ListWorker = tmp;
        }
        #endregion

        #region Methods
        internal bool CheckAllDone()
        {
            if(isDone)
                return true;
            //
            frameCount++;
            if(Timeout > 0 && Time.time - StartTime > Timeout)
            {
                foreach(var handler in listWorkerHandler)
                    handler.Timeout();
            }
            else
            {
                bool isAllDone = true;
                foreach(var handler in listWorkerHandler)
                {
                    if(!handler.IsDone)
                    {
                        isAllDone = false;
                        break;
                    }
                }
                //
                if(!isAllDone)
                {
                    if(FrameLimit > 0 && frameCount >= FrameLimit)
                    {
                        foreach(var handler in listWorkerHandler)
                            handler.Timeout();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            // callback
            isDone = true;
            try{
                callback?.Invoke(ListWorker);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
            // return true
            return true;
        }
        #endregion
    }
}
