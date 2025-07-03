using KTool.Attribute;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KTool.Init
{
    public class InitContainer : MonoBehaviour
    {
        #region Properties
        private const string LOG_INIT_CONTAINER_COMPLETE = "Init container complete: {0} - time[{1}]";

        [SerializeField]
        private float timeLimit = 5;
        [SerializeField]
        private InitStep[] steps;
        [SerializeField]
        private bool afterInit;
        [SerializeField, SelectScene]
        private string nextScene;
        [SerializeField]
        private LoadSceneMode loadSceneMode;
        [SerializeField]
        private UnityEvent onBegin,
            onEnd;

        private bool isIniting;
        private float initTime;

        public float TimeLimit => timeLimit;
        public int Count => steps.Length;
        public InitStep this[int index] => steps[index];
        public bool AfterInit => afterInit;
        public string NextScene => nextScene;
        public LoadSceneMode LoadSceneMode => loadSceneMode;
        #endregion

        #region Methods Unity
        private void Update()
        {
            if (isIniting)
                initTime += Time.unscaledDeltaTime;
        }
        #endregion

        #region Methods
        internal void PushEvent_OnBegin()
        {
            isIniting = true;
            initTime = 0;
            onBegin?.Invoke();
        }
        internal void PushEvent_OnEnd()
        {
            isIniting = false;
            Debug.Log(string.Format(LOG_INIT_CONTAINER_COMPLETE, name, initTime));
            onEnd?.Invoke();
        }
        #endregion
    }
}
