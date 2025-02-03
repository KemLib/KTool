using Firebase.RemoteConfig;
using KTool.Init;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace KPlugin.Firebase.RemoteConfig
{
    public class FirebaseRemoteConfigControl : MonoBehaviour, IInit
    {
        #region Properties
        public static FirebaseRemoteConfigControl Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private InitType initType = InitType.Optional;
        [SerializeField]
        private bool isDebugData;
        [SerializeField]
        [DataConfig]
        private DataConfig[] datas;


        private bool initComplete;
        private bool isAvailable;
        private List<string> updatedKeys;
        public delegate void OnKeyUpdateData(string[] keys);
        public event OnKeyUpdateData OnRemoteConfigUpdate;

        public string Name => gameObject.name;
        public InitType InitType => initType;
        public bool InitComplete => initComplete;
        public bool IsAvailable => isAvailable;
        public int Count => datas.Length;
        public DataConfig this[int index] => datas[index];
        public DataConfig this[string key]
        {
            get
            {
                foreach (DataConfig data in datas)
                    if (data.Key == key)
                        return data;
                return null;
            }
        }
        private FirebaseRemoteConfig InstanceRemoteConfig => FirebaseRemoteConfig.DefaultInstance;
        #endregion

        #region Unity Event
        private void OnDestroy()
        {
            if (IsAvailable)
                InstanceRemoteConfig.OnConfigUpdateListener -= Firebase_OnConfigUpdateListener;
        }
        #endregion

        #region Init
        public void InitBegin()
        {
            if (InitComplete)
                return;
            //
            Instance = this;
            updatedKeys = new List<string>();
            //
            Firebase_Init();
        }
        public void InitEnd()
        {

        }
        #endregion

        #region Method
        private void DebugData()
        {
            if (!isDebugData)
                return;
            //
            foreach (DataConfig data in datas)
            {
                string value;
                switch (data.DataType)
                {
                    case DataType.String:
                        value = data.ValueString;
                        break;
                    case DataType.Long:
                        value = data.ValueLong.ToString();
                        break;
                    case DataType.Double:
                        value = data.ValueDouble.ToString();
                        break;
                    case DataType.Boolean:
                        value = data.ValueBoolean.ToString();
                        break;
                    case DataType.Json:
                        value = data.ValueJson;
                        break;
                    default:
                        value = string.Empty;
                        break;
                }
                string log = string.Format("Key[{0}] - Type[{1}] - Value: {2}", data.Key, data.DataType, value);
                Debug.Log(log);
            }
        }
        #endregion

        #region Firebase
        private void Firebase_Init()
        {
            StartCoroutine(Firebase_IE_Init());
        }
        private IEnumerator Firebase_IE_Init()
        {
            while (FirebaseManager.Instance == null || !FirebaseManager.Instance.InitComplete)
                yield return new WaitForEndOfFrame();
            //
            if (!FirebaseManager.Instance.IsAvailable)
            {
                isAvailable = false;
                initComplete = true;
                yield break;
            }
            isAvailable = true;
            InstanceRemoteConfig.OnConfigUpdateListener += Firebase_OnConfigUpdateListener;
            //
            Dictionary<string, object> defaultData = Firebase_CreateDefaultData();
            Task taskSetDefaultData = InstanceRemoteConfig.SetDefaultsAsync(defaultData);
            while (!taskSetDefaultData.IsCompleted)
                yield return new WaitForEndOfFrame();
            //
            Firebase_FetchData(0);
        }
        private void Firebase_FetchData(float delay)
        {
            StartCoroutine(Firebase_IE_FetchData(delay));
        }
        private IEnumerator Firebase_IE_FetchData(float delay)
        {
            if (delay > 0)
                yield return new WaitForSecondsRealtime(delay);
            //
            Task task = InstanceRemoteConfig.FetchAsync(System.TimeSpan.Zero);
            while (!task.IsCompleted)
                yield return new WaitForEndOfFrame();
            //
            ConfigInfo info = InstanceRemoteConfig.Info;
            if (info.LastFetchStatus == LastFetchStatus.Success)
            {
                Firebase_ActivateData(0);
            }
            else
            {
                Firebase_FetchData(1);
            }
        }
        private void Firebase_ActivateData(float delay = 0)
        {
            StartCoroutine(Firebase_IE_ActivateData(delay));
        }
        private IEnumerator Firebase_IE_ActivateData(float delay)
        {
            if (delay > 0)
                yield return new WaitForSecondsRealtime(delay);
            //
            Task<bool> task = InstanceRemoteConfig.ActivateAsync();
            while (!task.IsCompleted)
                yield return new WaitForEndOfFrame();
            //
            if (!InitComplete)
            {
                initComplete = true;
                foreach (DataConfig data in datas)
                    data.Init(InstanceRemoteConfig);
            }
            if (updatedKeys.Count > 0)
            {
                OnRemoteConfigUpdate?.Invoke(updatedKeys.ToArray());
                updatedKeys.Clear();
            }
            DebugData();
        }
        private void Firebase_OnConfigUpdateListener(object sender, ConfigUpdateEventArgs args)
        {
            if (args.Error == RemoteConfigError.None)
                return;
            //
            foreach (string key in args.UpdatedKeys)
                updatedKeys.Add(key);
            Firebase_ActivateData(0);
        }
        private Dictionary<string, object> Firebase_CreateDefaultData()
        {
            Dictionary<string, object> defaultData = new Dictionary<string, object>();
            foreach (var data in datas)
            {
                if (string.IsNullOrEmpty(data.Key))
                    continue;
                switch (data.DataType)
                {
                    case DataType.String:
                        defaultData.Add(data.Key, data.DefaultValueString);
                        break;
                    case DataType.Long:
                        defaultData.Add(data.Key, data.DefaultValueLong);
                        break;
                    case DataType.Double:
                        defaultData.Add(data.Key, data.DefaultValueDouble);
                        break;
                    case DataType.Boolean:
                        defaultData.Add(data.Key, data.DefaultValueBoolean);
                        break;
                    case DataType.Json:
                        defaultData.Add(data.Key, data.DefaultValueJson);
                        break;
                }
            }
            //
            return defaultData;
        }
        #endregion
    }
}
