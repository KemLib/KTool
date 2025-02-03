using UnityEngine;

namespace KTool.Data
{
    public abstract class DataItem : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private string key;

        private DataManager dataManager;
        private DataDic data;

        public string Key => (string.IsNullOrEmpty(key) ? name : key);
        protected DataDic Data => data;
        #endregion Properties

        #region UnityEvent

        #endregion UnityEvent

        #region Method
        public void Init(DataDic data)
        {
            this.data = data;
            OnInit();
        }

        protected abstract void OnInit();
        #endregion Method
    }
}
