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
        private GroupItem[] groups;
        [SerializeField]
        public UnityEvent OnInitBegin,
            OnInitEnd;
        [SerializeField]
        public UnityEvent<float> OnProgress;
        private bool isLoading;
        private float progress;

        public bool IsLoading
        {
            get => isLoading;
            private set
            {
                isLoading = value;
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
        #endregion

        #region Unity Event
        private void Awake()
        {
            if (Instance != null)
            {
                if (Instance.GetInstanceID() != GetInstanceID())
                    Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
                return;
            }
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
            foreach (GroupItem group in groups)
                group.Init();
            InitBegin();
        }
        #endregion

        #region Loading
        private void InitBegin()
        {
            IsLoading = true;
            Progress = 0;
            //
            if (groups == null || groups.Length == 0)
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
            foreach (GroupItem group in groups)
                group.Item_InitEnded();
            //
            Progress = 1;
            IsLoading = false;
        }
        private IEnumerator IE_Init_TimeLimit()
        {
            float stepProgress = 1f / groups.Length;
            //
            float time = 0;
            for (int i = 0; i < groups.Length; i++)
            {
                GroupItem group = groups[i];
                group.Item_Init();
                do
                {
                    yield return new WaitForEndOfFrame();
                    if (time < maxTime)
                        time = Mathf.Min(time + Time.unscaledDeltaTime, maxTime);
                    float currentProgress = i * stepProgress,
                        progressGroup = group.Item_GetProgress(),
                        progressItem = currentProgress + progressGroup * stepProgress,
                        progressTime = time / maxTime;
                    Progress = progressItem * 0.9f + progressTime * 0.1f;
                    //
                    if (time >= maxTime && group.Item_IsCompleteAllCompulsory())
                    {
                        StartCoroutine(IE_Init_Now(i + 1));
                        yield break;
                    }
                } while (!group.Item_IsCompleteAll());
            }
            //
            InitEnd();
        }
        private IEnumerator IE_Init_TimeUnLimit()
        {
            float stepProgress = 1f / groups.Length;
            //
            for (int i = 0; i < groups.Length; i++)
            {
                GroupItem group = groups[i];
                group.Item_Init();
                float currentProgress = i * stepProgress,
                    progressGroup;
                do
                {
                    yield return new WaitForEndOfFrame();
                    progressGroup = group.Item_GetProgress();
                    Progress = currentProgress + progressGroup * stepProgress;
                } while (!group.Item_IsCompleteAll());
            }
            //
            InitEnd();
        }

        private IEnumerator IE_Init_Now(int indexStart)
        {
            float stepProgress = 1f / groups.Length;
            //
            for (int i = indexStart; i < groups.Length; i++)
            {
                GroupItem group = groups[i];
                group.Item_Init();
                float currentProgress = i * stepProgress;
                //
                Progress = currentProgress * 0.9f + 0.1f;
                while (!group.Item_IsCompleteAllCompulsory())
                    yield return new WaitForEndOfFrame();
                Progress = (currentProgress + stepProgress) * 0.9f + 0.1f;
            }
            //
            InitEnd();
        }
        #endregion
    }
}
