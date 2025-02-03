using System.Collections.Generic;
using UnityEngine;

namespace KTool.Data
{
    public class DataManager : MonoBehaviour
    {
        #region Properties
        private const string KEY_DATA_DEFAULT = "DataManager";

        [SerializeField]
        private string key = KEY_DATA_DEFAULT;
        [SerializeField]
        private bool isDontDestroy = true;

        private DataItem[] items;
        private DataDic data;

        private string Key => (string.IsNullOrEmpty(key) ? name : key);
        public int Count => items.Length;
        public DataItem this[int index]
        {
            get
            {
                if (index < 0 || index >= items.Length)
                    return null;
                return items[index];
            }
        }
        #endregion Properties

        #region UnityEvent
        private void Awake()
        {
            if (isDontDestroy)
                DontDestroyOnLoad(gameObject);
            Init();
        }
        #endregion UnityEvent

        #region Method
        public void Init()
        {
            data = new DataDic(Key);
            items = GetDataItem_Inchild();
            foreach (var unit in items)
            {
                DataDic unitData = data.GetKDic(unit.Key);
                unit.Init(unitData);
            }
        }

        public T GetDataItem<T>() where T : DataItem
        {
            foreach (var unit in items)
            {
                if (unit is T)
                    return unit as T;
            }
            return null;
        }
        private DataItem[] GetDataItem_Inchild()
        {
            List<DataItem> items = new List<DataItem>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform tfChild = transform.GetChild(i);
                DataItem item = tfChild.GetComponent<DataItem>();
                if (item != null)
                    items.Add(item);
            }
            return items.ToArray();
        }
        #endregion Method
    }
}
