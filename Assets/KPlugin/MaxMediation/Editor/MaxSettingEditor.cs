using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KPlugin.MaxMediation.Editor
{
    [CustomEditor(typeof(MaxSetting))]
    public class MaxSettingEditor : UnityEditor.Editor
    {
        #region Properties
        public const string ASSET_MAX_SETTING_FOLDER_NAME = "Assets/KPlugin/MaxMediation/Resources/" + MaxSetting.RESOURCES_PATH_FOLDER,
            ASSET_MAX_SETTING_FILE_NAME = MaxSetting.RESOURCES_PATH_FILE;
        public const string ASSET_MAX_SETTING_PATH = ASSET_MAX_SETTING_FOLDER_NAME + "/" + ASSET_MAX_SETTING_FILE_NAME + ".asset";

        private MaxMediationSettingEditor maxMediationSetting;
        private MaxSettingIdEditor appOpenSetting,
            bannerSetting,
            mRectSetting,
            interstitialSetting,
            rewardedSetting;
        #endregion

        #region Unity Event
        private void OnEnable()
        {
            Init();
        }

        public override void OnInspectorGUI()
        {
            OnGui_GoogleAdMobSetting();
            GUILayout.Space(5);
            OnGui_AdMobSetting();
        }
        private void OnGui_GoogleAdMobSetting()
        {
            maxMediationSetting.OnInspectorGUI();
        }
        private void OnGui_AdMobSetting()
        {
            serializedObject.Update();
            //
            appOpenSetting.OnInspectorGUI();
            GUILayout.Space(5);
            bannerSetting.OnInspectorGUI();
            GUILayout.Space(5);
            mRectSetting.OnInspectorGUI();
            GUILayout.Space(5);
            interstitialSetting.OnInspectorGUI();
            GUILayout.Space(5);
            rewardedSetting.OnInspectorGUI();
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Method
        private void Init()
        {
            //googleAdMobSetting = new GoogleAdMobSettingEditor();
            SerializedProperty propertyAppOpenIds = serializedObject.FindProperty("appOpenIds"),
                propertyBannerIds = serializedObject.FindProperty("bannerIds"),
                propertyMRecIds = serializedObject.FindProperty("mRecIds"),
                propertyInterstitialIds = serializedObject.FindProperty("interstitialIds"),
                propertyRewardedIds = serializedObject.FindProperty("rewardedIds");
            maxMediationSetting = new MaxMediationSettingEditor(serializedObject);
            appOpenSetting = new MaxSettingIdEditor(propertyAppOpenIds, MaxAdType.AppOpen);
            bannerSetting = new MaxSettingIdEditor(propertyBannerIds, MaxAdType.Banner);
            mRectSetting = new MaxSettingIdEditor(propertyMRecIds, MaxAdType.MRec);
            interstitialSetting = new MaxSettingIdEditor(propertyInterstitialIds, MaxAdType.Interstitial);
            rewardedSetting = new MaxSettingIdEditor(propertyRewardedIds, MaxAdType.Rewarded);
        }
        #endregion
    }
}
