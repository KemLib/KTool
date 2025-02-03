using KTool.Attribute;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KTool.Init
{
    public class LoadScene : MonoBehaviour, IInit
    {
        #region Properties
        private const float MAX_LOAD_PROGRESS = 0.9f;

        [SerializeField]
        [SelectScene]
        private string scene;
        [SerializeField]
        private LoadSceneMode mode;

        private AsyncOperation aoLoadScene;

        public string Name => gameObject.name;
        public InitType InitType => InitType.Compulsory;
        public bool InitComplete => (aoLoadScene == null ? false : aoLoadScene.progress >= MAX_LOAD_PROGRESS);
        #endregion

        #region Unity Event

        #endregion

        #region Method
        public void InitBegin()
        {
            aoLoadScene = SceneManager.LoadSceneAsync(scene, mode);
            aoLoadScene.allowSceneActivation = false;
        }

        public void InitEnd()
        {
            aoLoadScene.allowSceneActivation = true;
        }
        #endregion
    }
}
