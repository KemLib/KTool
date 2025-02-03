using GoogleMobileAds.Api;
using KTool.Init;
using System.Collections.Generic;
using UnityEngine;

namespace KPlugin.AdMob
{
    public class AdMobManager : MonoBehaviour, IInit
    {
        #region Properties
        public const string ADMOB_SCOURCE = "GoogleAdMob",
            ADMOB_COUNTRY_CODE = "UnknownCountry";

        public static AdMobManager Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private InitType initType;
        [SerializeField]
        private string[] testDeviceIds;

        private bool isInitBegin,
            isInitEnd,
            initComplete;

        public string Name => gameObject.name;
        public bool InitComplete => initComplete;
        public InitType InitType => initType;
        #endregion

        #region Unity Event

        #endregion

        #region Method

        #endregion

        #region Init
        public void InitBegin()
        {
            if (isInitBegin)
                return;
            isInitBegin = true;
            //
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                //
                AdMobSetting.Init();
                AdMob_Init();
                return;
            }
            if (Instance.GetInstanceID() == GetInstanceID())
                return;
            initComplete = true;
        }

        public void InitEnd()
        {
            if (isInitEnd)
                return;
            isInitEnd = true;
        }
        #endregion

        #region AdMob
        private void AdMob_Init()
        {
            MobileAds.Initialize(AdMob_OnInitComplete);
        }
        private void AdMob_OnInitComplete(InitializationStatus initStatus)
        {
            initComplete = true;
            //
            AdMob_RequestTestDevice();
        }

        private void AdMob_RequestTestDevice()
        {
            List<string> ids = new List<string>();
            foreach (string deviceId in testDeviceIds)
            {
                if (string.IsNullOrEmpty(deviceId))
                    continue;
                ids.Add(deviceId);
            }
            if (ids.Count == 0)
                return;
            //
            RequestConfiguration requestConfiguration = new RequestConfiguration();
            foreach (string deviceId in ids)
                requestConfiguration.TestDeviceIds.Add(deviceId);
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }
        #endregion
    }
}
