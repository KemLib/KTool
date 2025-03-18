using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KTool.Attribute;
using KTool.Loading;

namespace KTool_Demo.Loading
{
    public class LoadUi : MonoBehaviour, ILoadUi
    {
        #region Properties
        public static LoadUi Instance
        {
            get;
            private set;
        }
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private TextMeshProUGUI txtTaskName;
        [SerializeField]
        private Image imtProgress;
        [SerializeField]
        private TextMeshProUGUI txtProgress;

        public float Progress
        {
            get => imtProgress.fillAmount;
            set
            {
                imtProgress.fillAmount = value;
                txtProgress.text = string.Format(ILoadUi.TEXT_PROGRESS_FORMAT, Mathf.FloorToInt(imtProgress.fillAmount * 100));
            }
        }
        public string TaskName
        {
            get => txtTaskName.text;
            set => txtTaskName.text = value;
        }
        public bool IsShow => canvas.gameObject.activeSelf;
        public bool IsChanging => false;
        #endregion

        #region Unity Event	
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                ILoadUi.Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
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

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Method

        #endregion

        #region Menu Anim
        public void Show()
        {
            if (IsShow)
                return;
            canvas.gameObject.SetActive(true);
        }
        public void Show(float time)
        {
            if (IsShow)
                return;
            canvas.gameObject.SetActive(true);
        }
        public void Hide()
        {
            if (!IsShow)
                return;
            canvas.gameObject.SetActive(false);
        }
        public void Hide(float time)
        {
            if (!IsShow)
                return;
            canvas.gameObject.SetActive(false);
        }
        #endregion
    }
}
