using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Data
{
    public class DataDic : DataObject
    {
        #region Properties

        #endregion Properties

        #region Constructor
        public DataDic(string keyRoot) : base(keyRoot)
        {

        }
        #endregion Constructor

        #region Key
        public bool Key_Has(string keyContent)
        {
            string key = Key_Get(keyContent);
            return PlayerPrefs.HasKey(key);
        }
        public void Key_Delete(string keyContent)
        {
            string key = Key_Get(keyContent);
            PlayerPrefs.DeleteKey(key);
        }
        #endregion Key

        #region PlayerPrefs Get
        public int GetInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(Key_Get(key), defaultValue);
        }
        public float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(Key_Get(key), defaultValue);
        }
        public string Get(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(Key_Get(key), defaultValue);
        }
        public bool Get(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(Key_Get(key), defaultValue ? 1 : 0) == 1;
        }
        public Vector2 Get(string key, Vector2 defaultValue)
        {
            string keyChildren = Key_Get(key);
            DataVecter2 data = new DataVecter2(keyChildren, defaultValue);
            return data.Value;
        }
        public Vector2Int Get(string key, Vector2Int defaultValue)
        {
            string keyChildren = Key_Get(key);
            DataVecter2Int data = new DataVecter2Int(keyChildren, defaultValue);
            return data.Value;
        }
        public Vector3 Get(string key, Vector3 defaultValue)
        {
            string keyChildren = Key_Get(key);
            DataVecter3 data = new DataVecter3(keyChildren, defaultValue);
            return data.Value;
        }
        public Vector3Int Get(string key, Vector3Int defaultValue)
        {
            string keyChildren = Key_Get(key);
            DataVecter3Int data = new DataVecter3Int(keyChildren, defaultValue);
            return data.Value;
        }
        public DateTime Get(string key, DateTime defaultValue)
        {
            return ConvertToDate(PlayerPrefs.GetString(Key_Get(key), ConvertToString(defaultValue)));
        }
        public Dictionary<string, object> Get(string key, Dictionary<string, object> defaultValue)
        {
            return ConvertToDictionary(PlayerPrefs.GetString(Key_Get(key), ConvertToString(defaultValue)));
        }
        public List<object> Get(string key, List<object> defaultValue)
        {
            return ConvertToList(PlayerPrefs.GetString(Key_Get(key), ConvertToString(defaultValue)));
        }
        public DataDic GetKDic(string key)
        {
            string keyChildren = Key_Get(key);
            DataDic newKDictionary = new DataDic(keyChildren);
            return newKDictionary;
        }
        public DataList GetKList(string key, int count)
        {
            string keyChildren = Key_Get(key);
            DataList newDataList = new DataList(keyChildren, count);
            return newDataList;
        }
        #endregion PlayerPrefs Get

        #region PlayerPrefs Set
        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(Key_Get(key), value);
        }
        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(Key_Get(key), value);
        }
        public void Set(string key, string value)
        {
            PlayerPrefs.SetString(Key_Get(key), value);
        }
        public void Set(string key, bool value)
        {
            PlayerPrefs.SetInt(Key_Get(key), value ? 1 : 0);
        }
        public void Set(string key, Vector2 value)
        {
            string keyChildren = Key_Get(key);
            DataVecter2 data = new DataVecter2(keyChildren);
            data.Value = value;
        }
        public void Set(string key, Vector2Int value)
        {
            string keyChildren = Key_Get(key);
            DataVecter2Int data = new DataVecter2Int(keyChildren);
            data.Value = value;
        }
        public void Set(string key, Vector3 value)
        {
            string keyChildren = Key_Get(key);
            DataVecter3 data = new DataVecter3(keyChildren);
            data.Value = value;
        }
        public void Set(string key, Vector3Int value)
        {
            string keyChildren = Key_Get(key);
            DataVecter3Int data = new DataVecter3Int(keyChildren);
            data.Value = value;
        }
        public void Set(string key, DateTime value)
        {
            PlayerPrefs.SetString(Key_Get(key), ConvertToString(value));
        }
        public void Set(string key, Dictionary<string, object> value)
        {
            PlayerPrefs.SetString(Key_Get(key), ConvertToString(value));
        }
        public void Set(string key, List<object> value)
        {
            PlayerPrefs.SetString(Key_Get(key), ConvertToString(value));
        }
        #endregion PlayerPrefs Set
    }
}
