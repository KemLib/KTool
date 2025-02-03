using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Init;

namespace KPlugin.MaxMediation
{
    public class MaxManager : MonoBehaviour, IInit
    {
        #region Properties
        public const string MAX_SCOURCE = "MaxMediation",
            MAX_CURRENCY = "usd";

        public static MaxManager Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private InitType initType;
        [SerializeField]
        [Min(2)]
        private int delayCompleteInit;
        [SerializeField]
        private bool showDebugger;

        private bool isInitBegin;
        private bool initComplete;
        private string countryCode;

        public string Name => gameObject.name;
        public InitType InitType => initType;
        public bool InitComplete => initComplete;
        public bool AdMute
        {
            get => MaxSdk.IsMuted();
            set => MaxSdk.SetMuted(value);
        }
        public string CountryCode => countryCode;
        #endregion

        #region Unity Event
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
                MaxSetting.Init();
                Max_Setup();
                Max_Init();
                return;
            }
            if (Instance.GetInstanceID() == GetInstanceID())
                return;
            initComplete = true;
        }

        public void InitEnd()
        {

        }
        #endregion

        #region Method

        #endregion

        #region Max
        private void Max_Setup()
        {
            MaxSdk.SetSdkKey(MaxSetting.Instance.SdkKey);
            if (!string.IsNullOrEmpty(MaxSetting.Instance.UserId))
                MaxSdk.SetUserId(MaxSetting.Instance.UserId);
            if (!string.IsNullOrEmpty(MaxSetting.Instance.UserSegment))
                MaxSdk.UserSegment.Name = MaxSetting.Instance.UserSegment;
            MaxSdkCallbacks.OnSdkInitializedEvent += Max_OnSdkInitializedEvent;
        }
        private void Max_Init()
        {
            MaxSdk.InitializeSdk();
        }
        private void Max_OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            if (MaxSdk.IsInitialized())
                StartCoroutine(IE_CompleteInit());
            else
                StartCoroutine(IE_MaxInit());
        }
        private IEnumerator IE_MaxInit()
        {
            yield return new WaitForEndOfFrame();
            Max_Init();
        }
        private IEnumerator IE_CompleteInit()
        {
            if (delayCompleteInit > 0)
                yield return new WaitForSeconds(delayCompleteInit);
            //
            initComplete = true;
            if (showDebugger)
                MaxSdk.ShowMediationDebugger();
            countryCode = MaxSdk.GetSdkConfiguration().CountryCode;
        }
        #endregion
    }
}
