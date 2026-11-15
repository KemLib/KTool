using KTool.Cron;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

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
        private bool autoInit;
        [SerializeField]
        private UnityEvent onBegin,
            onProgress,
            onSuccess,
            onFail;
        
        private InitContainer currentInitContainer;
        private AsyncOperation currentLoadScene;
        private float originalProgress,
            initProgress,
            sceneProgress,
            currentProgress;
        
        public bool IsInitializing => currentInitContainer != null || currentLoadScene != null;
        public float Progress => currentProgress;
        #endregion

        #region Unity Event
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void OnDestroy()
        {
            if (Instance != null && Instance.GetEntityId() == GetEntityId())
                Instance = null;
        }
        private void Start()
        {
            if(Instance == null || Instance.GetEntityId() != GetEntityId())
                return;
            if(autoInit)
                Init();
        }
        private void Update()
        {
            if(!IsInitializing)
                return;
            //
            float newProgress = 0;
            if(currentInitContainer != null)
                newProgress = originalProgress + initProgress * currentInitContainer.Progress;
            else if(currentLoadScene != null)
                newProgress = originalProgress + initProgress + sceneProgress * currentLoadScene.progress;
            if(newProgress > currentProgress)
            {
                currentProgress = newProgress;
                onProgress?.Invoke();
            }
        }
        #endregion

        #region Method

        #endregion

        #region Init
        public void Init()
        {
            Scene scene = GetSceneActive();
            Init(scene);
        }
        public void Init(int sceneIndex)
        {
            Scene scene = GetScene(sceneIndex);
            Init(scene);
        }
        public void Init(string sceneName)
        {
            Scene scene = GetScene(sceneName);
            Init(scene);
        }
        public void Init(Scene scene)
        {
            if(IsInitializing)
            {
                Debug.LogError("Init is already in progress");
                return;
            }
            if(!scene.IsValid())
            {
                Debug.LogError($"Scene is not valid: {scene.name}");
                return;
            }
            if(!scene.isLoaded)
            {
                Debug.LogError($"Scene is not loaded: {scene.name}");
                return;
            }
            //
            InitContainer initContainer = GetComponent<InitContainer>(scene);
            if (initContainer == null)
            {
                Debug.LogError($"InitContainer is not found in scene: {scene.name}");
                return;
            }
            if(initContainer.IsInitting || initContainer.IsInited)
            {
                Debug.LogError("Init is already in progress or inited");
                return;
            }
            //
            Init(initContainer);
        }
        private void Init(InitContainer initContainer)
        {
            currentInitContainer = initContainer;
            originalProgress = 0;
            initProgress = initContainer.NextScene ? 0.5f * 0.8f : 1;
            sceneProgress = initContainer.NextScene ? 0.5f * 0.2f : 0;
            currentProgress = 0;
            //
            onBegin?.Invoke();
            //
            initContainer.Init_Begin(Init_End);
        }
        private void Init_End(InitContainer initContainer)
        {
            if(!initContainer.IsInited)
            {
                Debug.LogError("Init is not inited");
                return;
            }
            //
            currentInitContainer = null;
            if(!initContainer.IsSuccessfully)
            {
                Init_Fail();
                return;
            }
            //
            currentProgress = originalProgress + initProgress;
            onProgress?.Invoke();
            //
            if(initContainer.NextScene)
                LoadScene_Begin(initContainer.NextSceneName, initContainer.LoadSceneMode);
            else
                Init_Success();
        }
        private void Init_Success()
        {
            currentProgress = 1;
            onProgress?.Invoke();
            //
            onSuccess?.Invoke();
        }
        private void Init_Fail()
        {
            currentProgress = 1;
            onProgress?.Invoke();
            //
            onFail?.Invoke();
        }
        #endregion

        #region Load Scene
        private void LoadScene_Begin(string sceneName, LoadSceneMode loadSceneMode)
        {
            try{
                currentLoadScene = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
                currentLoadScene = null;
                Init_Fail();
                return;
            }
            if(currentLoadScene == null)
            {
                Debug.LogError("Failed to load scene");
                Init_Fail();
                return;
            }
            //
            currentLoadScene.allowSceneActivation = true;
            CronObject.Create()
                .Add(ConditionAsyncOperation.Create(currentLoadScene))
                .Add(CallbackAction.Create(LoadScene_End, sceneName))
                .Run();
        }
        private void LoadScene_End(string sceneName)
        {
            currentLoadScene = null;
            //
            Scene scene = GetScene(sceneName);
            if(!scene.IsValid())
            {
                Debug.LogError($"Scene is not valid: {sceneName}");
                Init_Fail();
                return;
            }
            if(!scene.isLoaded)
            {
                Debug.LogError($"Scene is not loaded: {sceneName}");
                Init_Fail();
                return;
            }
            InitContainer initContainer = GetComponent<InitContainer>(scene);
            if (initContainer == null)
            {
                Debug.LogError($"InitContainer is not found in scene: {sceneName}");
                Init_Success();
                return;
            }
            if(initContainer.IsInitting || initContainer.IsInited)
            {
                Debug.LogError("Init is already in progress or inited");
                Init_Fail();
                return;
            }
            //
            currentInitContainer = initContainer; 
            //
            currentProgress = originalProgress + initProgress + sceneProgress;
            onProgress?.Invoke();
            //
            originalProgress = currentProgress;
            if(initContainer.NextScene)
            {
                float nextProgress = (1 - originalProgress) / 2;
                initProgress = nextProgress * 0.8f;
                sceneProgress = nextProgress - initProgress;
            }
            else
            {
                initProgress = 1 - originalProgress;
                sceneProgress = 0;
            }
            initContainer.Init_Begin(Init_End);
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
        private static Scene GetScene(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName);
        }
        private static Scene GetScene(int sceneIndex)
        {
            return SceneManager.GetSceneByBuildIndex(sceneIndex);
        }
        private static Scene GetSceneActive()
        {
            return SceneManager.GetActiveScene();
        }
        #endregion
    }
}
