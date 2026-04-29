using KTool.Cron;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        #endregion

        #region Method

        #endregion

        #region Init
        private void Init(Scene scene)
        {
            InitContainer initContainer = GetComponent<InitContainer>(scene);
            Init(initContainer);
        }
        private void Init(InitContainer initContainer)
        {
            if (initContainer == null)
                return;
            //
            initContainer.Init_Begin();
            if (initContainer.Count <= 0)
            {
                Init_End(initContainer);
            }
            else
            {
                CronObject.Create()
                    .Add(ConditionDelegate.Create(initContainer.Init_Update))
                    .Add(CallbackAction.Create(Init_End, initContainer))
                    .Run();
            }
        }
        private void Init_End(InitContainer initContainer)
        {
            initContainer.Init_End();
        }
        #endregion

        #region Load Scene
        public void LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            AsyncOperation async_operation = SceneManager.LoadSceneAsync(sceneName, sceneMode);
            async_operation.allowSceneActivation = true;
            CronObject.Create()
                .Add(ConditionAsyncOperation.Create(async_operation))
                .Add(CallbackAction.Create(LoadScene_End, sceneName))
                .Run();
        }
        public void LoadScene(int sceneIndex, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            AsyncOperation async_operation = SceneManager.LoadSceneAsync(sceneIndex, sceneMode);
            async_operation.allowSceneActivation = true;
            CronObject.Create()
                .Add(ConditionAsyncOperation.Create(async_operation))
                .Add(CallbackAction.Create(LoadScene_End, sceneIndex))
                .Run();
        }
        private void LoadScene_End(int sceneIndex)
        {
            Scene scene = GetScene(sceneIndex);
            LoadScene_End(scene);
        }
        private void LoadScene_End(string sceneName)
        {
            Scene scene = GetSceneActive(sceneName);
            LoadScene_End(scene);
        }
        private void LoadScene_End(Scene scene)
        {
            InitContainer initContainer = GetComponent<InitContainer>(scene);
            if (initContainer != null)
                Init(initContainer);
        }
        #endregion

        #region Utillity
        private static T GetComponent<T>(Scene scene) where T : Component
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
        private static Scene GetSceneActive(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName);
        }
        private static Scene GetSceneActive()
        {
            return SceneManager.GetActiveScene();
        }
        private static Scene GetScene(int sceneIndex)
        {
            return SceneManager.GetSceneByBuildIndex(sceneIndex);
        }
        #endregion
    }
}
