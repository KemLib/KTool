using UnityEngine;
using UnityEngine.UI;

namespace KTool.Ui.Popup
{
    public class LoadUiDefault : MonoBehaviour, ILoadUi
    {
        #region  Properties
        private const string RESOURCES_PATH = "KTool/Ui/Popup/LoadUiDefault";
        private const string GAME_OBJECT_NAME = "KTool_Ui_LoadUiDefault";
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
        private Image panelMenu;
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
        public bool IsShow => panelMenu.gameObject.activeSelf;
        public bool IsStateChanging => false;
        #endregion

        #region  Unity Event
        private void OnDestroy()
        {
            if (instance != null && instance.GetInstanceID() == GetInstanceID())
                instance = null;
        }
        #endregion

        #region Menu Anim
        public void Show()
        {
            if (IsShow)
                return;
            panelMenu.gameObject.SetActive(true);
        }
        public void Show(float time)
        {
            if (IsShow)
                return;
            panelMenu.gameObject.SetActive(true);
        }
        public void Hide()
        {
            if (!IsShow)
                return;
            panelMenu.gameObject.SetActive(false);
        }
        public void Hide(float time)
        {
            if (!IsShow)
                return;
            panelMenu.gameObject.SetActive(false);
        }
        #endregion
    }
}
