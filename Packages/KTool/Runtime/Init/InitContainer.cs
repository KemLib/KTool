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
        private UnityEvent onStep,
            onProgress,
            onBegin,
            onEnd;

        private int index_step;
        private float progress;
        private float stepProgress;

        public float TimeLimit => timeLimit;
        public int Count => steps.Length;
        public InitStep this[int index] => steps[index];
        internal float StepProgress => stepProgress;
        public float Progress => progress;
        public InitStep CurrentStep => index_step < 0 || index_step >= Count ? null : steps[index_step];
        #endregion

        #region Methods Unity

        #endregion

        #region Methods
        internal void PushEvent_OnStep(int index_step)
        {
            this.index_step = index_step;
            //
            onStep?.Invoke();
        }
        internal void PushEvent_OnProgress()
        {
            float tmpProgress = stepProgress * index_step + stepProgress * CurrentStep.Item_GetProgress();
            if (tmpProgress == progress)
                return;
            progress = Mathf.Clamp(tmpProgress, 0, 1);
            //
            onStep?.Invoke();
        }
        internal void PushEvent_OnBegin()
        {
            index_step = -1;
            progress = -1;
            stepProgress = Count <= 0 ? 0 : 1f / Count;
            //
            onBegin?.Invoke();
        }
        internal void PushEvent_OnEnd()
        {
            onEnd?.Invoke();
        }
        #endregion
    }
}
