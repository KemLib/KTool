using UnityEngine;
using UnityEngine.SceneManagement;

namespace KTool.Init
{
    public class InitManager : MonoBehaviour
    {
        private enum InitState
        {
            None,
            Init_Component,
            Load_Scene
        }
        #region Properties
        private const string LOAD_SCENE_TASK_NAME_FORMAT = "Load scene: {0}";
        public static InitManager Instance
        {
            get;
            private set;
        }

        private InitState state;
        private InitContainer initContainer;
        private float initStartTime;
        private int index_step;
        private AsyncOperation async_operation;
        private string sceneName;
        private int sceneIndex;

        public bool IsInit => state != InitState.None;
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
            switch (state)
            {
                case InitState.None:
                    break;
                case InitState.Init_Component:
                    Init_Update();
                    break;
                case InitState.Load_Scene:
                    LoadScene_Update();
                    break;
            }
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
            {
                state = InitState.None;
                return;
            }
            //
            state = InitState.Init_Component;
            this.initContainer = initContainer;
            //
            initContainer.PushEvent_OnBegin();
            if (initContainer.Count == 0)
            {
                Init_End();
                return;
            }
            //
            initStartTime = Time.time;
            Init_NextStep(0);
        }
        private void Init_End()
        {
            InitContainer tmpInitContainer = initContainer;
            state = InitState.None;
            initContainer = null;
            //
            for (int i = 0; i < tmpInitContainer.Count; i++)
                tmpInitContainer[i].Item_InitEnded();
            tmpInitContainer.PushEvent_OnEnd();
        }
        private void Init_Update()
        {
            if (index_step >= initContainer.Count)
            {
                Init_End();
                return;
            }
            //
            initContainer.PushEvent_OnProgress();
            InitStep step = initContainer[index_step];
            if (initContainer.TimeLimit > 0)
            {
                float delta_time = Time.time - initStartTime;
                if (!step.Item_IsCompleteAllRequired() || (delta_time < initContainer.TimeLimit && !step.Item_IsCompleteAll()))
                {
                    return;
                }
            }
            else
            {
                if (!step.Item_IsCompleteAll())
                {
                    return;
                }
            }
            //
            Init_NextStep(index_step + 1);
        }
        private void Init_NextStep(int index_step)
        {
            if (index_step >= initContainer.Count)
            {
                Init_End();
                return;
            }
            //
            this.index_step = index_step;
            initContainer.PushEvent_OnStep(index_step);
            //
            InitStep step = initContainer[index_step];
            step.Init();
            step.Item_Init();
        }
        #endregion

        #region Load Scene
        public void LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            state = InitState.Load_Scene;
            //
            this.sceneName = sceneName;
            sceneIndex = -1;
            //
            async_operation = SceneManager.LoadSceneAsync(sceneName, sceneMode);
            async_operation.allowSceneActivation = true;
        }
        public void LoadScene(int sceneIndex, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            state = InitState.Load_Scene;
            //
            sceneName = string.Empty;
            this.sceneIndex = sceneIndex;
            //
            async_operation = SceneManager.LoadSceneAsync(sceneIndex, sceneMode);
            async_operation.allowSceneActivation = true;
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
            sceneName = string.Empty;
            sceneIndex = -1;
            async_operation = null;
            //
            Init(initContainer);
        }
        private void LoadScene_End()
        {
            sceneName = string.Empty;
            sceneIndex = -1;
            async_operation = null;
            //
            state = InitState.None;
        }
        private void LoadScene_Update()
        {
            if (async_operation == null)
            {
                LoadScene_End();
                return;
            }
            //
            if (async_operation.isDone)
            {
                Scene scene;
                if (string.IsNullOrEmpty(sceneName))
                    scene = GetScene(sceneIndex);
                else
                    scene = GetSceneActive(sceneName);
                LoadScene_End(scene);
            }
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
