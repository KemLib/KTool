using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.RemoteConfig;

namespace KPlugin.Firebase.RemoteConfig
{
    [System.Serializable]
    public class DataConfig
    {
        #region Properties
        [SerializeField]
        private string key;
        [SerializeField]
        private DataType dataType;
        [SerializeField]
        private string valueString;
        [SerializeField]
        private long valueLong;
        [SerializeField]
        private double valueDouble;
        [SerializeField]
        private bool valueBoolean;
        [SerializeField]
        private string valueJson;

        private FirebaseRemoteConfig instance;

        public string Key => key;
        public DataType DataType => dataType;
        public string DefaultValueString => valueString;
        public long DefaultValueLong => valueLong;
        public double DefaultValueDouble => valueDouble;
        public bool DefaultValueBoolean => valueBoolean;
        public string DefaultValueJson => valueJson;
        public string ValueString => (string.IsNullOrEmpty(Key) || instance == null ? valueString : instance.GetValue(Key).StringValue);
        public long ValueLong => (string.IsNullOrEmpty(Key) || instance == null ? valueLong : instance.GetValue(Key).LongValue);
        public double ValueDouble => (string.IsNullOrEmpty(Key) || instance == null ? valueDouble : instance.GetValue(Key).DoubleValue);
        public bool ValueBoolean => (string.IsNullOrEmpty(Key) || instance == null ? valueBoolean : instance.GetValue(Key).BooleanValue);
        public string ValueJson => (string.IsNullOrEmpty(Key) || instance == null ? valueJson : instance.GetValue(Key).StringValue);
        #endregion

        #region Method
        public void Init(FirebaseRemoteConfig instance)
        {
            this.instance = instance;
        }
        #endregion
    }
}
