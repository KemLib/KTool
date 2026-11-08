using System;
using KTool.Attribute;
using KTool.Cron;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KTool.Init
{
    public class InitContainer : MonoBehaviour
    {
        private enum State
        {
            None,
            Initting,
            Success,
            Fail
        }
        #region Properties
        private const int STATE_NONE = (int)State.None;
        private const int STATE_INITTING = (int)State.Initting;
        private const int STATE_SUCCESS = (int)State.Success;
        private const int STATE_FAIL = (int)State.Fail;

        [SerializeField]
        private float timeLimit = 5;
        [SerializeField]
        private InitStep[] steps;
        [SerializeField]
        private bool nextScene;
        [SerializeField, SelectScene]
        private string nextSceneName;
        [SerializeField]
        private LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        [SerializeField]
        private UnityEvent onBegin,
            onStep,
            onProgress,
            onEnd;

        private int state = STATE_NONE;
        private float initStartTime;
        private float initTime;
        private int stepCount;
        private int stepIndex;
        private float progress;
        private float progressPerStep;

        public float TimeLimit => timeLimit;
        public bool NextScene => nextScene;
        public string NextSceneName => nextSceneName;
        public LoadSceneMode LoadSceneMode => loadSceneMode;
        public int Count => stepCount;
        public int Step => stepIndex;
        public float Progress => progress;
        public float InitTime => initTime;
        private InitStep CurrentStep => steps[stepIndex];
        public bool IsInitting => state == STATE_INITTING;
        public bool IsInited => state == STATE_SUCCESS || state == STATE_FAIL;
        public bool IsSuccessfully => state == STATE_SUCCESS;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods
        internal void Init_Begin(Action<InitContainer> callback)
        {
            if(IsInitting || IsInited)
            {
                Debug.LogError("Init is already in progress or inited");
                callback?.Invoke(this);
                return;
            }
            state = STATE_INITTING;
            //
            initStartTime = Time.realtimeSinceStartup;
            initTime = 0;
            if (steps == null || steps.Length <= 0)
            {
                stepCount = 0;
                stepIndex = 0;
                progress = 0;
                progressPerStep = 0;
                //
                onBegin?.Invoke();
                //
                progress = 1;
                onProgress?.Invoke();
                //
                state = STATE_SUCCESS;
                onEnd?.Invoke();
                //
                callback?.Invoke(this);
                return;
            }
            //
            stepCount = steps.Length;
            stepIndex = 0;
            progress = 0;
            progressPerStep = stepCount <= 0 ? 0 : 1f / stepCount;
            //
            onBegin?.Invoke();
            //
            Init_Step(0);
            PushEvent_OnProgress();
            //
            CronObject.Create()
                .Add(ConditionDelegate.Create(Init_Update))
                .Add(CallbackAction.Create(Init_End, callback))
                .Run();
        }
        private void Init_End(Action<InitContainer> callback)
        {
            for (int i = 0; i <= stepIndex && i  < stepCount; i++)
            {
                if(steps[i] == null)
                    continue;
                steps[i].Item_InitEnded();
            }
            //
            onEnd?.Invoke();
            //
            callback?.Invoke(this);
        }
        private bool Init_Update()
        {
            initTime = Time.realtimeSinceStartup - initStartTime;
            PushEvent_OnProgress();
            //
            bool isSuccessIndispensable = true;
            if (TimeLimit > 0 && initTime >= TimeLimit)
            {
                if (CurrentStep == null || CurrentStep.Item_IsCompleteIndispensable(out isSuccessIndispensable))
                {
                    if(!isSuccessIndispensable)
                    {
                        Debug.Log($"Init failed: {name}");
                        state = STATE_FAIL;
                        return true;
                    }
                    if (Init_IsComplete())
                    {
                        state = STATE_SUCCESS;
                        return true;
                    }
                    Init_Step(stepIndex + 1);
                }
            }
            else
            {
                if (CurrentStep == null || CurrentStep.Item_IsCompleteAll(out isSuccessIndispensable))
                {
                    if(!isSuccessIndispensable)
                    {
                        Debug.Log($"Init failed: {name}");
                        state = STATE_FAIL;
                        return true;
                    }
                    if (Init_IsComplete())
                    {
                        state = STATE_SUCCESS;
                        return true;
                    }
                    Init_Step(stepIndex + 1);
                }
            }
            //
            return false;
        }
        private bool Init_IsComplete()
        {
            return stepIndex >= stepCount - 1;
        }
        private void Init_Step(int stepIndex)
        {
            this.stepIndex = stepIndex;
            if(CurrentStep != null)
            {
                CurrentStep.Init();
                CurrentStep.Item_Init();
            }
            //
            onStep?.Invoke();
        }
        private void PushEvent_OnProgress()
        {
            float stepProgress = CurrentStep == null ? 1: CurrentStep.Item_GetProgress(),
                tmpProgress = progressPerStep * stepIndex + progressPerStep * stepProgress;
            if (tmpProgress > progress)
            {
                progress = Mathf.Clamp(tmpProgress, 0, 1);
                //
                onProgress?.Invoke();
            }
        }
        #endregion
    }
}
