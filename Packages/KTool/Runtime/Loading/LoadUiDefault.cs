using UnityEngine;
using UnityEngine.UI;

namespace KTool.Loading
{
    public class LoadUiDefault : MonoBehaviour, ILoadUi
    {
        #region  Properties
        private const string RESOURCES_PATH = "KTool/Loading/LoadUiDefault";
        private const string GAME_OBJECT_NAME = "KTool_LoadUiDefault";
        private static LoadUiDefault instance;
        public static LoadUiDefault Instance
        {
            get
            {
                if (instance == null)
                {
                    LoadUiDefault prefab = Resources.Load<LoadUiDefault>(RESOURCES_PATH);
                    instance = Instantiate(prefab);
                    instance.gameObject.name = GAME_OBJECT_NAME;
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        [SerializeField]
        private Image imgProgress;

        public float Progress
        {
            get => imgProgress.fillAmount;
            set => imgProgress.fillAmount = value;
        }
        public string TaskName
        {
            get => string.Empty;
            set
            {

            }
        }
        public bool IsShow => gameObject.activeSelf;
        public bool IsChanging => false;
        #endregion

        #region  Unity Event

        #endregion

        #region Menu Anim
        public void Show()
        {
            if (gameObject.activeSelf)
                return;
            gameObject.SetActive(true);
        }
        public void Show(float time)
        {
            if (gameObject.activeSelf)
                return;
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            if (!gameObject.activeSelf)
                return;
            gameObject.SetActive(false);
        }
        public void Hide(float time)
        {
            if (!gameObject.activeSelf)
                return;
            gameObject.SetActive(false);
        }
        #endregion
    }
}
