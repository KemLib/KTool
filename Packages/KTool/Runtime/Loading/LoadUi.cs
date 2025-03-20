using UnityEngine;
using UnityEngine.UI;

namespace KTool.Loading
{
    public class LoadUi : MonoBehaviour, ILoadUi
    {
        #region  Properties
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
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
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
