using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KTool.Init
{
    public class InitManager : MonoBehaviour
    {
        #region Properties
        private const string LOG_INIT_COMPLETE = "Init complete: {0} - time[{1}]";
        private const string LOAD_SCENE_TASK_NAME_FORMAT = "Load scene: {0}";
        public static InitManager Instance
        {
            get;
            private set;
        }


        [SerializeField]
        private UnityEvent onInit,
            onComplete;
        [SerializeField]
        private UnityEvent<float> onProgress;
        [SerializeField]
        private UnityEvent<string> onTaskName;

        private bool isInit;
        private float progress;
        private string taskName;
        private float initTime;

        public bool IsInit
        {
            get => isInit;
            private set
            {
                if (value == isInit)
                    return;
                //
                isInit = value;
                if (isInit)
                {
                    initTime = 0;
                    onInit?.Invoke();
                }
                else
                {
                    Debug.Log(string.Format(LOG_INIT_COMPLETE, name, initTime));
                    onComplete?.Invoke();
                }
            }
        }
        public float Progress
        {
            get => progress;
            private set
            {
                if (value == progress)
                    return;
                progress = Mathf.Clamp(value, 0, 1);
                onProgress?.Invoke(progress);
            }
        }
        public string TaskName
        {
            get => taskName;
            private set
            {
                if (value == taskName)
                    return;
                taskName = value;
                onTaskName?.Invoke(taskName);
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
        private void Start()
        {
            Scene scene = GetSceneActive();
            Init(scene);
        }
        private void Update()
        {
            if (IsInit)
                initTime += Time.unscaledDeltaTime;
        }
        #endregion

        #region Method

        #endregion

        #region Init FirstScene
        private void Init(Scene scene)
        {
            InitContainer initContainer = GetComponent<InitContainer>(scene);
            if (initContainer == null)
                return;
            //
            IsInit = true;
            Progress = 0;
            TaskName = string.Empty;
            //
            if (initContainer.Count == 0)
                Init_End(initContainer);
            else if (initContainer.TimeLimit > 0)
                StartCoroutine(Init_TimeLimit(initContainer));
            else
                StartCoroutine(Init_TimeUnLimit(initContainer));
        }
        private void Init_End(InitContainer initContainer)
        {
            for (int i = 0; i < initContainer.Count; i++)
                initContainer[i].Item_InitEnded();
            initContainer.PushEvent_OnEnd();
            //
            if (initContainer.AfterInit)
            {
                StartCoroutine(LoadScene_IE(initContainer.NextScene, initContainer.LoadSceneMode));
            }
            else
            {
                Progress = 1;
                TaskName = string.Empty;
                IsInit = false;
            }
        }
        private IEnumerator Init_TimeLimit(InitContainer initContainer)
        {
            initContainer.PushEvent_OnBegin();
            //
            float originProgress = progress,
                maxProgress = (1 - originProgress) / (initContainer.AfterInit ? 3 : 1),
                stepProgress = maxProgress / initContainer.Count;
            //
            float time = 0;
            for (int i = 0; i < initContainer.Count; i++)
            {
                InitStep step = initContainer[i];
                step.Init();
                TaskName = step.StepName;
                step.Item_Init();
                //
                while (!step.Item_IsCompleteAllRequired() || (time < initContainer.TimeLimit && !step.Item_IsCompleteAll()))
                {
                    yield return new WaitForEndOfFrame();
                    Progress = originProgress + stepProgress * i + stepProgress * step.Item_GetProgress();
                    time = Mathf.Min(time + Time.unscaledDeltaTime, initContainer.TimeLimit);
                }
                //
                Progress = originProgress + stepProgress * i + stepProgress;
                TaskName = string.Empty;
            }
            //
            Init_End(initContainer);
        }
        private IEnumerator Init_TimeUnLimit(InitContainer initContainer)
        {
            initContainer.PushEvent_OnBegin();
            //
            float originProgress = progress,
                maxProgress = (1 - originProgress) / (initContainer.AfterInit ? 3 : 1),
                stepProgress = maxProgress / initContainer.Count;
            //
            for (int i = 0; i < initContainer.Count; i++)
            {
                InitStep step = initContainer[i];
                step.Init();
                TaskName = step.StepName;
                step.Item_Init();
                //
                while (!step.Item_IsCompleteAll())
                {
                    yield return new WaitForEndOfFrame();
                    Progress = originProgress + stepProgress * i + stepProgress * step.Item_GetProgress();
                }
                //
                Progress = originProgress + stepProgress * i + stepProgress;
                TaskName = string.Empty;
            }
            //
            Init_End(initContainer);
        }
        #endregion

        #region Load Scene
        public void LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            IsInit = true;
            Progress = 0;
            TaskName = string.Empty;
            StartCoroutine(LoadScene_IE(sceneName, sceneMode));
        }
        public void LoadScene(int sceneindex, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            IsInit = true;
            Progress = 0;
            TaskName = string.Empty;
            StartCoroutine(LoadScene_IE(sceneindex, sceneMode));
        }
        private void LoadScene_End(Scene scene)
        {
            InitContainer initContainer = GetComponent<InitContainer>(scene);
            if (initContainer == null || initContainer.Count == 0)
            {
                LoadScene_End();
                return;
            }
            //
            if (initContainer.TimeLimit > 0)
                StartCoroutine(Init_TimeLimit(initContainer));
            else
                StartCoroutine(Init_TimeUnLimit(initContainer));
        }
        private void LoadScene_End()
        {
            Progress = 1;
            TaskName = string.Empty;
            IsInit = false;
        }
        private IEnumerator LoadScene_IE(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, sceneMode);
            if (ao == null)
            {
                LoadScene_End();
                yield break;
            }
            //
            TaskName = string.Format(LOAD_SCENE_TASK_NAME_FORMAT, sceneName);
            ao.allowSceneActivation = true;
            //
            float originProgress = progress,
                maxProgress = (1 - originProgress) / 2;
            while (!ao.isDone)
            {
                Progress = originProgress + maxProgress * ao.progress;
                yield return new WaitForEndOfFrame();
            }
            Progress = originProgress + maxProgress;
            TaskName = string.Empty;
            //
            Scene scene = GetScene(sceneName);
            LoadScene_End(scene);
        }
        private IEnumerator LoadScene_IE(int sceneIndex, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneIndex, sceneMode);
            if (ao == null)
            {
                LoadScene_End();
                yield break;
            }
            //
            TaskName = string.Format(LOAD_SCENE_TASK_NAME_FORMAT, sceneIndex);
            ao.allowSceneActivation = true;
            //
            float originProgress = progress,
                maxProgress = (1 - originProgress) / 2;
            while (!ao.isDone)
            {
                Progress = originProgress + maxProgress * ao.progress;
                yield return new WaitForEndOfFrame();
            }
            Progress = originProgress + maxProgress;
            TaskName = string.Empty;
            //
            Scene scene = GetScene(sceneIndex);
            LoadScene_End(scene);
        }
        #endregion

        #region Utillity
        public static T GetComponent<T>(Scene scene) where T : Component
        {
            if (!scene.IsValid() || !scene.isLoaded)
                return null;
            GameObject[] rootGO = scene.GetRootGameObjects();
            foreach (GameObject go in rootGO)
            {
                if (!go.activeSelf)
                    continue;
                T item = go.GetComponentInChildren<T>();
                if (item != null)
                    return item;
            }
            return null;
        }
        public static Scene GetScene(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName);
        }
        public static Scene GetScene(int sceneIndex)
        {
            return SceneManager.GetSceneByBuildIndex(sceneIndex);
        }
        public static Scene GetSceneActive()
        {
            return SceneManager.GetActiveScene();
        }
        #endregion
    }
}
