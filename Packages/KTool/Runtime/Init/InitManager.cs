using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using KTool.Ui.Popup;
using System;
using System.Collections.Generic;
using KTool.Attribute;

namespace KTool.Init
{
    public class InitManager : MonoBehaviour
    {
        #region Properties
        private const string LOAD_SCENE_TASK_NAME_FORMAT = "Load scene: {0}";
        public static InitManager Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private float initTimeLimit = 5;
        [SerializeField]
        private InitStep[] steps;
        [SerializeField, SelectScene]
        private string nextScene;
        [SerializeField]
        private LoadSceneMode loadSceneMode;

        private bool isIniting;

        public bool IsIniting
        {
            get => isIniting;
            private set => isIniting = value;
        }
        public ILoadUi LoadUi
        {
            get
            {
                if (ILoadUi.Instance == null)
                    return LoadUiDefault.Instance;
                return ILoadUi.Instance;
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
#if UNITY_EDITOR
            Scene scene = GetSceneActive();
            if (scene.buildIndex == 0)
                InitFirstScene();
            else
            {
                IsIniting = true;
                LoadUi.Progress = 0;
                Init(LoadUi, scene);
            }
#else
            InitFirstScene();
#endif
        }
        #endregion

        #region Method

        #endregion

        #region Init FirstScene
        public void InitFirstScene()
        {
            foreach (InitStep step in steps)
                step.Init();
            IsIniting = true;
            LoadUi.Progress = 0;
            InitFirstScene_Begin(LoadUi);
        }
        private void InitFirstScene_Begin(ILoadUi loadUi)
        {
            if (steps == null || steps.Length == 0)
            {
                InitFirstScene_End(loadUi);
                return;
            }
            //
            if (initTimeLimit > 0)
                StartCoroutine(InitFirstScene_TimeLimit(loadUi));
            else
                StartCoroutine(InitFirstScene_TimeUnLimit(loadUi));
        }
        private void InitFirstScene_End(ILoadUi loadUi)
        {
            foreach (InitStep step in steps)
                step.Item_InitEnded();
            //
            LoadScene(loadUi, nextScene, loadSceneMode);
        }
        private IEnumerator InitFirstScene_TimeLimit(ILoadUi loadUi)
        {
            if (!loadUi.IsShow)
            {
                loadUi.TaskName = string.Empty;
                loadUi.Show();
            }
            while (loadUi.IsStateChanging)
                yield return new WaitForEndOfFrame();
            //
            float originProgress = loadUi.Progress,
                maxProgress = (1 - originProgress) / 3,
                stepProgress = maxProgress / steps.Length;
            //
            float time = 0;
            for (int i = 0; i < steps.Length; i++)
            {
                InitStep step = steps[i];
                loadUi.TaskName = step.StepName;
                step.Item_Init();
                //
                while (true)
                {
                    yield return new WaitForEndOfFrame();
                    loadUi.Progress = originProgress + stepProgress * i + stepProgress * step.Item_GetProgress();
                    //
                    time = Mathf.Min(time + Time.unscaledDeltaTime, initTimeLimit);
                    //
                    if (time > initTimeLimit)
                    {
                        if (step.Item_IsCompleteAllRequired())
                            break;
                    }
                    else if (step.Item_IsCompleteAll())
                        break;
                }
            }
            //
            loadUi.Progress = originProgress + maxProgress;
            InitFirstScene_End(loadUi);
        }
        private IEnumerator InitFirstScene_TimeUnLimit(ILoadUi loadUi)
        {
            if (!loadUi.IsShow)
            {
                loadUi.TaskName = string.Empty;
                loadUi.Show();
            }
            while (loadUi.IsStateChanging)
                yield return new WaitForEndOfFrame();
            //
            float originProgress = loadUi.Progress,
                maxProgress = (1 - originProgress) / 3,
                stepProgress = maxProgress / steps.Length;
            //
            for (int i = 0; i < steps.Length; i++)
            {
                InitStep step = steps[i];
                loadUi.TaskName = step.StepName;
                step.Item_Init();
                //
                while (true)
                {
                    yield return new WaitForEndOfFrame();
                    loadUi.Progress = originProgress + stepProgress * i + stepProgress * step.Item_GetProgress();
                    //
                    if (step.Item_IsCompleteAll())
                        break;
                }
            }
            //
            loadUi.Progress = originProgress + maxProgress;
            InitFirstScene_End(loadUi);
        }
        #endregion

        #region Load Scene
        public void LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            IsIniting = true;
            LoadUi.Progress = 0;
            StartCoroutine(LoadScene_IE(LoadUi, sceneName, sceneMode));
        }
        public void LoadScene(int sceneindex, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            IsIniting = true;
            LoadUi.Progress = 0;
            StartCoroutine(LoadScene_IE(LoadUi, sceneindex, sceneMode));
        }
        private void LoadScene(ILoadUi loadUi, string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            StartCoroutine(LoadScene_IE(loadUi, sceneName, sceneMode));
        }
        private IEnumerator LoadScene_IE(ILoadUi loadUi, string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            if (!loadUi.IsShow)
            {
                loadUi.TaskName = string.Empty;
                loadUi.Show();
            }
            while (loadUi.IsStateChanging)
                yield return new WaitForEndOfFrame();
            //
            loadUi.TaskName = string.Format(LOAD_SCENE_TASK_NAME_FORMAT, sceneName);
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, sceneMode);
            ao.allowSceneActivation = true;
            //
            float maxProgress = (1 - loadUi.Progress) / 2;
            while (!ao.isDone)
            {
                loadUi.Progress = maxProgress * ao.progress;
                yield return new WaitForEndOfFrame();
            }
            //
            loadUi.TaskName = string.Empty;
            Scene scene = GetScene(sceneName);
            Init(loadUi, scene);
        }
        private IEnumerator LoadScene_IE(ILoadUi loadUi, int sceneIndex, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            if (!loadUi.IsShow)
            {
                loadUi.TaskName = string.Empty;
                loadUi.Show();
            }
            while (loadUi.IsStateChanging)
                yield return new WaitForEndOfFrame();
            //
            loadUi.TaskName = string.Format(LOAD_SCENE_TASK_NAME_FORMAT, sceneIndex);
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneIndex, sceneMode);
            ao.allowSceneActivation = true;
            //
            float maxProgress = (1 - loadUi.Progress) / 2;
            while (!ao.isDone)
            {
                loadUi.Progress = maxProgress * ao.progress;
                yield return new WaitForEndOfFrame();
            }
            //
            loadUi.TaskName = string.Empty;
            Scene scene = GetScene(sceneIndex);
            Init(loadUi, scene);
        }
        #endregion

        #region Load Item
        private void Init(ILoadUi loadUi, Scene scene)
        {
            List<IIniter> items = GetComponents<IIniter>(scene);
            if (items.Count == 0)
            {
                if (loadUi.IsShow)
                {
                    loadUi.Progress = 1;
                    loadUi.TaskName = string.Empty;
                    loadUi.Hide();
                }
            }
            else
            {
                StartCoroutine(Init_Begin(loadUi, items));
            }
        }
        private IEnumerator Init_Begin(ILoadUi loadUi, List<IIniter> items)
        {
            if (!loadUi.IsShow)
            {
                loadUi.TaskName = string.Empty;
                loadUi.Show();
            }
            while (loadUi.IsStateChanging)
                yield return new WaitForEndOfFrame();
            //
            float currentProgress = loadUi.Progress,
                maxProgress = 1 - currentProgress,
                unitProgress = maxProgress / items.Count;
            //
            foreach (IIniter item in items)
            {
                TrackEntry trackLoader = item.InitBegin();
                loadUi.TaskName = trackLoader.Name;
                //
                while (!trackLoader.IsComplete)
                {
                    loadUi.Progress = currentProgress + unitProgress * trackLoader.Progress;
                    loadUi.TaskName = trackLoader.Name;
                    yield return new WaitForEndOfFrame();
                }
                //
                currentProgress += unitProgress;
                loadUi.Progress = currentProgress;
                loadUi.TaskName = string.Empty;
            }
            //
            loadUi.Progress = 1;
            loadUi.TaskName = string.Empty;
            StartCoroutine(Init_End(loadUi, items));
        }
        private IEnumerator Init_End(ILoadUi loadUi, List<IIniter> items)
        {
            if (loadUi.IsShow)
            {
                loadUi.Progress = 1;
                loadUi.TaskName = string.Empty;
                loadUi.Hide();
            }
            while (loadUi.IsStateChanging)
                yield return new WaitForEndOfFrame();
            //
            foreach (IIniter item in items)
                item.InitEnd();
            IsIniting = false;
        }
        #endregion

        #region Utillity
        public static List<T> GetComponents<T>(Scene scene)
        {
            List<T> components = new List<T>();
            if (!scene.IsValid() || !scene.isLoaded)
                return components;
            GameObject[] rootGO = scene.GetRootGameObjects();
            foreach (GameObject go in rootGO)
            {
                if (!go.activeSelf)
                    continue;
                T[] items = go.GetComponentsInChildren<T>();
                if (items.Length == 0)
                    continue;
                components.AddRange(items);
            }
            return components;
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
