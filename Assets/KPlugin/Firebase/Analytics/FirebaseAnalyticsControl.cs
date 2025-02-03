using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Init;
using Firebase.Analytics;

namespace KPlugin.Firebase.Analytics
{
    public class FirebaseAnalyticsControl : MonoBehaviour, IInit
    {
        #region Properties
        public static FirebaseAnalyticsControl Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private InitType initType;
        [SerializeField]
        private string userName,
            propertyName;


        private bool initComplete;
        private bool isAvailable;

        public string Name => gameObject.name;
        public InitType InitType => initType;
        public bool InitComplete => initComplete;
        public bool IsAvailable => isAvailable;
        public string UserName => userName;
        public string PropertyName => propertyName;
        #endregion

        #region Unity Event

        #endregion

        #region Method

        #endregion

        #region Init
        public void InitBegin()
        {
            if (InitComplete)
                return;
            //
            Instance = this;
            //
            Firebase_Init();
        }

        public void InitEnd()
        {

        }
        #endregion

        #region Log Event
        public static void LogEvent(string eventName)
        {
            if (Instance == null || !Instance.IsAvailable)
                return;
            FirebaseAnalytics.LogEvent(eventName);
        }
        public static void LogEvent(string eventName, string parameterName, string value)
        {
            if (Instance == null || !Instance.IsAvailable)
                return;
            FirebaseAnalytics.LogEvent(eventName, parameterName, value);
        }
        public static void LogEvent(string eventName, string parameterName, int value)
        {
            if (Instance == null || !Instance.IsAvailable)
                return;
            FirebaseAnalytics.LogEvent(eventName, parameterName, value);
        }
        public static void LogEvent(string eventName, string parameterName, long value)
        {
            if (Instance == null || !Instance.IsAvailable)
                return;
            FirebaseAnalytics.LogEvent(eventName, parameterName, value);
        }
        public static void LogEvent(string eventName, string parameterName, double value)
        {
            if (Instance == null || !Instance.IsAvailable)
                return;
            FirebaseAnalytics.LogEvent(eventName, parameterName, value);
        }
        public static void LogEvent(string eventName, Parameter[] parameters)
        {
            if (Instance == null || !Instance.IsAvailable)
                return;
            FirebaseAnalytics.LogEvent(eventName, parameters);
        }

        public static void SetUserProperty(string userName, string propertyName)
        {
            if (Instance == null || !Instance.IsAvailable)
                return;
            Instance.userName = userName;
            Instance.propertyName = propertyName;
            FirebaseAnalytics.SetUserProperty(userName, propertyName);
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
            isAvailable = FirebaseManager.Instance.IsAvailable;
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PropertyName))
                SetUserProperty(UserName, PropertyName);
            initComplete = true;
        }
        #endregion
    }
}
