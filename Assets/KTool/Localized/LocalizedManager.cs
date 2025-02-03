using KTool.Data;
using KTool.ThirdParty;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Localized
{
    public class LocalizedManager : MonoBehaviour
    {
        #region Properties
        private const string KEY_DATA_LOCALIZED = "LocalizedManager",
            KEY_DATA_LANGUAGE = KEY_DATA_LOCALIZED + "Language";

        public static LocalizedManager Instance
        {
            get;
            private set;
        }
        [SerializeField]
        private string keyData = KEY_DATA_LOCALIZED;
        [SerializeField]
        private SystemLanguage defaultLanguage = SystemLanguage.English;
        [SerializeField]
        private TextAsset[] languageAssets;
        [SerializeField]
        private LocalizedData[] localizeds;

        private bool isInit;
        private DataDic dataDic;
        private SystemLanguage currentLanguage;
        private string stringDefaultLanguage,
            stringCurrentLanguage;
        private LocalizedData defaultLocalizedData,
            currentLocalizedData;
        private List<TsvTable> tables;
        private event Action onInit,
            onChangeLanguage;

        private bool IsInit => isInit;
        public SystemLanguage LocaLanguage => Application.systemLanguage;
        public SystemLanguage DefaultLanguage => defaultLanguage;
        public SystemLanguage CurrentLanguage => currentLanguage;
        public string StringDefaultLanguage => stringDefaultLanguage;
        public string StringCurrentLanguage => stringCurrentLanguage;
        public LocalizedData DefaultLocalizedData => defaultLocalizedData;
        public LocalizedData CurrentLocalizedData => currentLocalizedData;
        #endregion Properties

        #region Unity Event
        private void Awake()
        {
            if (Instance == null)
            {
                Init();
                DontDestroyOnLoad(gameObject);
                return;
            }
            if (Instance.GetInstanceID() == GetInstanceID())
                return;
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if (Instance != null && Instance.GetInstanceID() == GetInstanceID())
                Instance = null;
        }
        #endregion Unity Event

        #region Method
        private void Init()
        {
            if (IsInit)
                return;
            //
            Instance = this;
            //
            string rootKeyData = (string.IsNullOrEmpty(keyData) ? KEY_DATA_LOCALIZED : keyData);
            dataDic = new DataDic(rootKeyData);
            currentLanguage = Data_GetCurrentLanguage();
            defaultLocalizedData = LocalizedData_Get(DefaultLanguage);
            currentLocalizedData = LocalizedData_Get(CurrentLanguage);
            stringDefaultLanguage = DefaultLanguage.ToString();
            stringCurrentLanguage = CurrentLanguage.ToString();
            //
            tables = new List<TsvTable>();
            foreach (TextAsset asset in languageAssets)
            {
                TsvTable table = TsvTable.Convert_StringToTsv(asset.text);
                tables.Add(table);
            }
            //
            isInit = true;
            onInit?.Invoke();
            if (CurrentLanguage != DefaultLanguage)
                onChangeLanguage?.Invoke();
        }
        public void SetCurrentLanguage(int index)
        {
            if (index < 0 || index >= localizeds.Length)
                return;
            LocalizedData data = localizeds[index];
            SetCurrentLanguage(data);
        }
        public void SetCurrentLanguage(LocalizedData data)
        {
            if (data == null)
                return;
            SetCurrentLanguage(data.Language);
        }
        public void SetCurrentLanguage(SystemLanguage language)
        {
            if (language == CurrentLanguage)
                return;
            if (!LocalizedData_Contains(language))
                return;
            currentLanguage = language;
            currentLocalizedData = LocalizedData_Get(CurrentLanguage);
            stringCurrentLanguage = CurrentLanguage.ToString();
            Data_SetCurrentLanguage(StringCurrentLanguage);
            //
            if (IsInit)
                onChangeLanguage?.Invoke();
        }
        #endregion Method

        #region Event
        public void Event_RegisterInit(Action onInit)
        {
            this.onInit += onInit;
            if (IsInit)
                onInit?.Invoke();
        }
        public void Event_RegisterChangeLanguage(Action onChangeLanguage)
        {
            this.onChangeLanguage += onChangeLanguage;
            if (IsInit)
                onChangeLanguage?.Invoke();
        }
        public void Event_Register(Action onInit, Action onChangeLanguage)
        {
            this.onInit += onInit;
            this.onChangeLanguage += onChangeLanguage;
            //
            if (IsInit)
            {
                onInit?.Invoke();
                onChangeLanguage?.Invoke();
            }
        }
        public void Event_Unregister(Action onInit, Action onChangeLanguage)
        {
            this.onInit -= onInit;
            this.onChangeLanguage -= onChangeLanguage;
        }
        #endregion

        #region Data
        private SystemLanguage Data_GetCurrentLanguage()
        {
            SystemLanguage language;
            string dataLanguage = dataDic.Get(KEY_DATA_LANGUAGE, LocaLanguage.ToString());
            if (Enum.TryParse<SystemLanguage>(dataLanguage, out language) && LocalizedData_Contains(language))
                return language;
            return DefaultLanguage;
        }
        private void Data_SetCurrentLanguage(string currentLanguage)
        {
            dataDic.Set(KEY_DATA_LANGUAGE, currentLanguage);
        }
        #endregion

        #region LocalizedData
        public int LocalizedData_Count()
        {
            return localizeds.Length;
        }
        public LocalizedData LocalizedData_Get(int index)
        {
            if (index < 0 || index >= localizeds.Length)
                return null;
            return localizeds[index];
        }
        public LocalizedData LocalizedData_Get(SystemLanguage language)
        {
            for (int i = 0; i < localizeds.Length; i++)
                if (localizeds[i].Language == language)
                    return localizeds[i];
            return null;
        }
        public int LocalizedData_GetIndexOf(SystemLanguage language)
        {
            for (int i = 0; i < localizeds.Length; i++)
                if (localizeds[i].Language == language)
                    return i;
            return -1;
        }
        public bool LocalizedData_Contains(SystemLanguage language)
        {
            for (int i = 0; i < localizeds.Length; i++)
                if (localizeds[i].Language == language)
                    return true;
            return false;
        }
        #endregion LocalizedData

        #region Language
        public void Language_GetRow(string valueDefault, out TsvTable table, out TsvRow row)
        {
            if (!IsInit)
            {
                table = null;
                row = null;
                return;
            }
            //
            TsvRow tsvRow;
            foreach (TsvTable tmpTable in tables)
            {
                int indexColume = tmpTable.Colume_GetIndex(StringDefaultLanguage);
                if (indexColume < 0)
                    continue;
                if (tmpTable.Row_Find(indexColume, valueDefault, out tsvRow))
                {
                    table = tmpTable;
                    row = tsvRow;
                    return;
                }
            }
            table = null;
            row = null;
            return;
        }
        #endregion Language
    }
}
