using KTool.Attribute;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private bool afterInit;
        [SerializeField, SelectScene]
        private string nextScene;
        [SerializeField]
        private LoadSceneMode loadSceneMode;

        public float TimeLimit => timeLimit;
        public int Count => steps.Length;
        public InitStep this[int index] => steps[index];
        public bool AfterInit => afterInit;
        public string NextScene => nextScene;
        public LoadSceneMode LoadSceneMode => loadSceneMode;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
