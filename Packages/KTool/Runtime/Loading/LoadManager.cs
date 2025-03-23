using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KTool.Loading
{
    public class LoadManager : MonoBehaviour
    {
        #region Properties
        private const string LOAD_SCENE_TASK_NAME_FORMAT = "Load scene: {0}";

        public static LoadManager Instance
        {
            get;
            private set;
        }

        [SerializeField]
        public bool IncludeInactive = false;

        public ILoadUi LoadUi => ILoadUi.Instance == null ? LoadUiDefault.Instance : ILoadUi.Instance;
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
            {
                Instance = null;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            LoadItem(LoadUi);
        }
        #endregion

        #region Load Scene
        public void LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            StartCoroutine(IE_LoadScene(LoadUi, sceneName));
        }
        public void LoadScene(int sceneindex, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            StartCoroutine(IE_LoadScene(LoadUi, sceneindex));
        }
        private IEnumerator IE_LoadScene(ILoadUi loadUi, string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            if (!loadUi.IsShow)
            {
                loadUi.Progress = 0;
                loadUi.TaskName = string.Empty;
                loadUi.Show();
            }
            while (loadUi.IsChanging)
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
            LoadItem(loadUi, scene);
        }
        private IEnumerator IE_LoadScene(ILoadUi loadUi, int sceneIndex, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            if (!loadUi.IsShow)
            {
                loadUi.Progress = 0;
                loadUi.TaskName = string.Empty;
                loadUi.Show();
            }
            while (loadUi.IsChanging)
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
            LoadItem(loadUi, scene);
        }
        #endregion

        #region Load Item
        private void LoadItem(ILoadUi loadUi)
        {
            Scene scene = GetSceneActive();
            List<ILoader> items = GetComponents<ILoader>(scene);
            if (items.Count == 0)
                return;
            //
            StartCoroutine(IE_LoadBegin(loadUi, items));
        }
        private void LoadItem(ILoadUi loadUi, Scene scene)
        {
            List<ILoader> items = GetComponents<ILoader>(scene);
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
                StartCoroutine(IE_LoadBegin(loadUi, items));
            }
        }
        private IEnumerator IE_LoadBegin(ILoadUi loadUi, List<ILoader> items)
        {
            if (!loadUi.IsShow)
            {
                loadUi.Progress = 0;
                loadUi.TaskName = string.Empty;
                loadUi.Show();
            }
            while (loadUi.IsChanging)
                yield return new WaitForEndOfFrame();
            //
            float currentProgress = loadUi.Progress,
                maxProgress = 1 - currentProgress,
                unitProgress = maxProgress / items.Count;
            //
            foreach (ILoader item in items)
            {
                TrackEntry trackLoader = item.LoadBegin();
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
            StartCoroutine(IE_GameStart(loadUi, items));
        }
        private IEnumerator IE_GameStart(ILoadUi loadUi, List<ILoader> items)
        {
            if (loadUi.IsShow)
            {
                loadUi.Progress = 1;
                loadUi.TaskName = string.Empty;
                loadUi.Hide();
            }
            while (loadUi.IsChanging)
                yield return new WaitForEndOfFrame();
            //
            foreach (ILoader item in items)
                item.LoadEnd();
        }
        #endregion

        #region Utillity
        public List<T> GetComponents<T>(Scene scene)
        {
            List<T> components = new List<T>();
            if (!scene.IsValid() || !scene.isLoaded)
                return components;
            GameObject[] rootGO = scene.GetRootGameObjects();
            foreach (GameObject go in rootGO)
            {
                if (!IncludeInactive && !go.activeSelf)
                    continue;
                //
                T[] items = go.GetComponentsInChildren<T>(IncludeInactive);
                if (items.Length == 0)
                    continue;
                components.AddRange(items);
            }
            return components;
        }
        public Scene GetScene(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName);
        }
        public Scene GetScene(int sceneIndex)
        {
            return SceneManager.GetSceneByBuildIndex(sceneIndex);
        }
        public Scene GetSceneActive()
        {
            return SceneManager.GetActiveScene();
        }
        #endregion
    }
}
