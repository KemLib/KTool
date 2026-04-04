using UnityEngine;
using UnityEngine.Events;

namespace KTool.Init
{
    public class InitContainer : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private float timeLimit = 5;
        [SerializeField]
        private InitStep[] steps;
        [SerializeField]
        private UnityEvent onBegin,
            onStep,
            onProgress,
            onEnd;

        private float initStartTime;
        private int index_step;
        private float progresTotal;
        private float progresStep;

        public float TimeLimit => timeLimit;
        public int Count => steps.Length;
        public int Step => index_step;
        public float ProgressStep => progresStep;
        public float ProgresTotal => progresTotal;
        public float CurrentTime => Time.time - initStartTime;
        private InitStep CurrentStep => steps[index_step];
        #endregion

        #region Methods Unity

        #endregion

        #region Methods
        internal void Init_Begin()
        {
            initStartTime = Time.time;
            index_step = 0;
            progresTotal = 0;
            progresStep = Count <= 0 ? 0 : 1f / Count;
            //
            onBegin?.Invoke();
            //
            if (Count > 0)
            {
                Init_Step(0);
                PushEvent_OnProgress();
            }
        }
        internal void Init_End()
        {
            for (int i = 0; i < steps.Length; i++)
                steps[i].Item_InitEnded();
            //
            onEnd?.Invoke();
        }
        internal bool Init_Update()
        {
            PushEvent_OnProgress();
            //
            if (TimeLimit > 0 && Time.time - initStartTime >= TimeLimit)
            {
                while (true)
                {
                    if (CurrentStep.Item_IsCompleteAllRequired())
                    {
                        if (Init_IsComplete())
                            return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //
            if (CurrentStep.Item_IsCompleteAll())
            {
                return Init_IsComplete();
            }
            else
            {
                return false;
            }
        }
        private bool Init_IsComplete()
        {
            if (index_step >= Count - 1)
                return true;
            Init_Step(index_step + 1);
            return false;
        }
        private void Init_Step(int index_step)
        {
            this.index_step = index_step;
            steps[index_step].Init();
            steps[index_step].Item_Init();
            //
            onStep?.Invoke();
        }
        private void PushEvent_OnProgress()
        {
            float tmpProgress = progresStep * index_step + progresStep * CurrentStep.Item_GetProgress();
            if (tmpProgress == progresTotal)
                return;
            progresTotal = Mathf.Clamp(tmpProgress, 0, 1);
            //
            onStep?.Invoke();
        }
        #endregion
    }
}
