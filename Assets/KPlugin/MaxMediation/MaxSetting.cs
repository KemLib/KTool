using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KPlugin.MaxMediation
{
    public class MaxSetting : ScriptableObject
    {
        #region Properties
        public const string RESOURCES_PATH_FOLDER = "KPlugin.MaxMediation",
            RESOURCES_PATH_FILE = "MaxSetting",
            RESOURCES_PATH = RESOURCES_PATH_FOLDER + "/" + RESOURCES_PATH_FILE;

        public static MaxSetting Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private string sdkKey;
        [SerializeField]
        private string userId,
            userSegment;
        [SerializeField]
        private MaxSettingId[] appOpenIds,
            bannerIds,
            mRecIds,
            interstitialIds,
            rewardedIds;

        public string SdkKey => sdkKey; 
        public string UserId => userId; 
        public string UserSegment => userSegment;
        #endregion

        #region Unity Event

        #endregion

        #region Method
        public static void Init()
        {
            Instance = GetInstance();
        }
        public static MaxSetting GetInstance()
        {
            return Resources.Load<MaxSetting>(RESOURCES_PATH);
        }
        #endregion

        #region Ad
        public int Ad_Count(MaxAdType adType)
        {
            switch (adType)
            {
                case MaxAdType.AppOpen:
                    return appOpenIds.Length;
                case MaxAdType.Banner:
                    return bannerIds.Length;
                case MaxAdType.MRec:
                    return mRecIds.Length;
                case MaxAdType.Interstitial:
                    return interstitialIds.Length;
                case MaxAdType.Rewarded:
                    return rewardedIds.Length;
                default:
                    return 0;
            }
        }
        public MaxSettingId Ad_Get(MaxAdType adType, int index)
        {
            switch (adType)
            {
                case MaxAdType.AppOpen:
                    return appOpenIds[index];
                case MaxAdType.Banner:
                    return bannerIds[index];
                case MaxAdType.MRec:
                    return mRecIds[index];
                case MaxAdType.Interstitial:
                    return interstitialIds[index];
                case MaxAdType.Rewarded:
                    return rewardedIds[index];
                default:
                    return null;
            }
        }
        #endregion
    }
}
