using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.Init
{
    public class InitManager : MonoBehaviour
    {
        #region Properties
        public static InitManager Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private float maxTime = 5;
        [SerializeField]
        private InitStep[] steps;
        [SerializeField]
        public UnityEvent OnInitBegin,
            OnInitEnd;
        [SerializeField]
        public UnityEvent<float> OnProgress;
        [SerializeField]
        public UnityEvent<string> OnTaskName;
        [SerializeField]
        public UnityEvent<string> OnFail;
        private bool isIniting;
        private float progress;
        private string taskName;

        public bool IsIniting
        {
            get => isIniting;
            private set
            {
                isIniting = value;
                if (value)
                    OnInitBegin?.Invoke();
                else
                    OnInitEnd?.Invoke();
            }
        }
        public float Progress
        {
            get => progress;
            private set
            {
                progress = value;
                OnProgress?.Invoke(progress);
            }
        }
        public string TaskName
        {
            get => taskName;
            set
            {
                taskName = value;
                OnTaskName?.Invoke(taskName);
            }
        }
        #endregion

        #region Unity Event
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
                return;
            }
            //
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if (Instance != null && Instance.GetInstanceID() == GetInstanceID())
                Instance = null;
        }
        #endregion

        #region Method
        public void Init()
        {
            foreach (InitStep step in steps)
                step.Init();
            InitBegin();
        }
        #endregion

        #region Loading
        private void InitBegin()
        {
            IsIniting = true;
            Progress = 0;
            //
            if (steps == null || steps.Length == 0)
            {
                InitEnd();
                return;
            }
            //
            if (maxTime > 0)
                StartCoroutine(IE_Init_TimeLimit());
            else
                StartCoroutine(IE_Init_TimeUnLimit());
        }
        private void InitEnd()
        {
            foreach (InitStep step in steps)
                step.Item_InitEnded(OnFail);
            //
            Progress = 1;
            IsIniting = false;
        }
        private IEnumerator IE_Init_TimeLimit()
        {
            float stepProgress = 1f / steps.Length;
            //
            float time = 0;
            for (int i = 0; i < steps.Length; i++)
            {
                InitStep step = steps[i];
                step.Item_Init(OnFail);
                TaskName = step.StepName;
                do
                {
                    yield return new WaitForEndOfFrame();
                    Progress = stepProgress * i + stepProgress * step.Item_GetProgress();
                    //
                    time = Mathf.Min(time + Time.unscaledDeltaTime, maxTime);
                    if (time >= maxTime && step.Item_IsCompleteAllRequired())
                    {
                        StartCoroutine(IE_Init_Now(i + 1));
                        yield break;
                    }
                } while (!step.Item_IsCompleteAll());
            }
            //
            InitEnd();
        }
        private IEnumerator IE_Init_TimeUnLimit()
        {
            float stepProgress = 1f / steps.Length;
            //
            for (int i = 0; i < steps.Length; i++)
            {
                InitStep step = steps[i];
                step.Item_Init(OnFail);
                TaskName = step.StepName;
                do
                {
                    yield return new WaitForEndOfFrame();
                    Progress = stepProgress * i + stepProgress * step.Item_GetProgress();
                } while (!step.Item_IsCompleteAll());
            }
            //
            InitEnd();
        }
        private IEnumerator IE_Init_Now(int indexStart)
        {
            float stepProgress = 1f / steps.Length;
            //
            for (int i = indexStart; i < steps.Length; i++)
            {
                InitStep step = steps[i];
                step.Item_Init(OnFail);
                TaskName = step.StepName;
                //
                while (!step.Item_IsCompleteAllRequired())
                {
                    Progress = stepProgress * i + stepProgress * step.Item_GetProgress();
                    yield return new WaitForEndOfFrame();
                }
                Progress = i * stepProgress + stepProgress;
            }
            //
            InitEnd();
        }
        #endregion
    }
}
