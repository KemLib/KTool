using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Data
{
    public class DataList : DataObject
    {
        #region Properties
        public const string ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION = "Index Out Of Range";
        public const string KEY_LIST_COUNT = "[COUNT]",
            KEY_LIST_INDEX = "[{0}]";

        private string keyCount;
        private int count;

        public int Count
        {
            get
            {
                return count;
            }
            private set
            {
                count = Mathf.Max(count, value);
                PlayerPrefs.SetInt(keyCount, count);
            }
        }
        #endregion Properties

        #region Constructor
        public DataList(string keyRoot, int count) : base(keyRoot)
        {
            keyCount = Key_Get(KEY_LIST_COUNT);
            this.count = PlayerPrefs.GetInt(keyCount, count);
        }
        #endregion Constructor

        #region Key
        private string Key_Get(int index)
        {
            string keyIndex = string.Format(KEY_LIST_INDEX, index);
            return Key_Get(keyIndex);
        }
        public bool Key_Has(int index)
        {
            string key = Key_Get(index);
            return PlayerPrefs.HasKey(key);
        }
        public void Key_Delete(int index)
        {
            string key = Key_Get(index);
            PlayerPrefs.DeleteKey(key);
        }
        #endregion Key

        #region PlayerPrefs Get
        public int GetInt(int index, int defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            return PlayerPrefs.GetInt(Key_Get(index), defaultValue);
        }
        public float GetFloat(int index, float defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            return PlayerPrefs.GetFloat(Key_Get(index), defaultValue);
        }
        public string Get(int index, string defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            return PlayerPrefs.GetString(Key_Get(index), defaultValue);
        }
        public bool Get(int index, bool defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            return PlayerPrefs.GetInt(Key_Get(index), defaultValue ? 1 : 0) == 1;
        }

        public Vector2 Get(int index, Vector2 defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            string keyChildren = Key_Get(index);
            DataVecter2 data = new DataVecter2(keyChildren, defaultValue);
            return data.Value;
        }
        public Vector2Int Get(int index, Vector2Int defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            string keyChildren = Key_Get(index);
            DataVecter2Int data = new DataVecter2Int(keyChildren, defaultValue);
            return data.Value;
        }
        public Vector3 Get(int index, Vector3 defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            string keyChildren = Key_Get(index);
            DataVecter3 data = new DataVecter3(keyChildren, defaultValue);
            return data.Value;
        }
        public Vector3Int Get(int index, Vector3Int defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            string keyChildren = Key_Get(index);
            DataVecter3Int data = new DataVecter3Int(keyChildren, defaultValue);
            return data.Value;
        }
        public DateTime Get(int index, DateTime defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            return ConvertToDate(PlayerPrefs.GetString(Key_Get(index), ConvertToString(defaultValue)));
        }
        public Dictionary<string, object> Get(int index, Dictionary<string, object> defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            return ConvertToDictionary(PlayerPrefs.GetString(Key_Get(index), ConvertToString(defaultValue)));
        }
        public List<object> Get(int index, List<object> defaultValue)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return defaultValue;
            }
            return ConvertToList(PlayerPrefs.GetString(Key_Get(index), ConvertToString(defaultValue)));
        }
        public DataDic GetKDic(int index)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return null;
            }
            string keyChildren = Key_Get(index);
            DataDic newDataObject = new DataDic(keyChildren);
            return newDataObject;
        }
        public DataList GetKList(int index, int count)
        {
            if (index < 0 || index >= Count)
            {
                Debug.LogWarning(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
                return null;
            }
            string keyChildren = Key_Get(index);
            DataList newDataList = new DataList(keyChildren, count);
            return newDataList;
        }
        #endregion PlayerPrefs Get

        #region PlayerPrefs Set
        public void SetInt(int index, int value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            PlayerPrefs.SetInt(Key_Get(index), value);
        }
        public void SetFloat(int index, float value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            PlayerPrefs.SetFloat(Key_Get(index), value);
        }
        public void Set(int index, string value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            PlayerPrefs.SetString(Key_Get(index), value);
        }
        public void Set(int index, bool value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            PlayerPrefs.SetInt(Key_Get(index), value ? 1 : 0);
        }
        public void Set(int index, Vector2 value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            string keyChildren = Key_Get(index);
            DataVecter2 data = new DataVecter2(keyChildren);
            data.Value = value;
        }
        public void Set(int index, Vector2Int value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            string keyChildren = Key_Get(index);
            DataVecter2Int data = new DataVecter2Int(keyChildren);
            data.Value = value;
        }
        public void Set(int index, Vector3 value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            string keyChildren = Key_Get(index);
            DataVecter3 data = new DataVecter3(keyChildren);
            data.Value = value;
        }
        public void Set(int index, Vector3Int value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            string keyChildren = Key_Get(index);
            DataVecter3Int data = new DataVecter3Int(keyChildren);
            data.Value = value;
        }
        public void Set(int index, DateTime value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            PlayerPrefs.SetString(Key_Get(index), ConvertToString(value));
        }
        public void Set(int index, Dictionary<string, object> value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            PlayerPrefs.SetString(Key_Get(index), ConvertToString(value));
        }
        public void Set(int index, List<object> value)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(ERROR_MESSAGE_INDEX_OUT_OF_RANGE_EXCEPTION);
            PlayerPrefs.SetString(Key_Get(index), ConvertToString(value));
        }
        #endregion PlayerPrefs Set

        #region PlayerPrefs Add
        public void AddInt(int value)
        {
            int index = Count++;
            PlayerPrefs.SetInt(Key_Get(index), value);
        }
        public void AddFloat(float value)
        {
            int index = Count++;
            PlayerPrefs.SetFloat(Key_Get(index), value);
        }
        public void Add(string value)
        {
            int index = Count++;
            PlayerPrefs.SetString(Key_Get(index), value);
        }
        public void Add(bool value)
        {
            int index = Count++;
            PlayerPrefs.SetInt(Key_Get(index), value ? 1 : 0);
        }
        public void Add(Vector2 value)
        {
            int index = Count++;
            string keyChildren = Key_Get(index);
            DataVecter2 data = new DataVecter2(keyChildren);
            data.Value = value;
        }
        public void Add(Vector2Int value)
        {
            int index = Count++;
            string keyChildren = Key_Get(index);
            DataVecter2Int data = new DataVecter2Int(keyChildren);
            data.Value = value;
        }
        public void Add(Vector3 value)
        {
            int index = Count++;
            string keyChildren = Key_Get(index);
            DataVecter3 data = new DataVecter3(keyChildren);
            data.Value = value;
        }
        public void Add(Vector3Int value)
        {
            int index = Count++;
            string keyChildren = Key_Get(index);
            DataVecter3Int data = new DataVecter3Int(keyChildren);
            data.Value = value;
        }
        public void Add(DateTime value)
        {
            int index = Count++;
            PlayerPrefs.SetString(Key_Get(index), ConvertToString(value));
        }
        public void Add(Dictionary<string, object> value)
        {
            int index = Count++;
            PlayerPrefs.SetString(Key_Get(index), ConvertToString(value));
        }
        public void Add(List<object> value)
        {
            int index = Count++;
            PlayerPrefs.SetString(Key_Get(index), ConvertToString(value));
        }
        #endregion PlayerPrefs Add
    }
}
