using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KTool.FileIo;

namespace KPlugin.MaxMediation.Editor
{
    public class MaxMediationSettingEditor
    {
        #region Properties
        private const string ASSET_MAX_SDK_SETTING_FOLDER_NAME = "Assets/MaxSdk/Resources",
            ASSET_MAX_SDK_SETTING_FILE_NAME = "AppLovinSettings";
        private const string ASSET_MAX_SDK_SETTING_PATH = ASSET_MAX_SDK_SETTING_FOLDER_NAME + "/" + ASSET_MAX_SDK_SETTING_FILE_NAME + ".asset";

        private SerializedObject serializedMaxMediation,
            serializedMaxSetting;
        private SerializedProperty propertyMaxMediationSdkKey;
        private SerializedProperty propertySdkKey,
            propertyUserId,
            propertyUserSegment;
        #endregion

        #region Construction
        public MaxMediationSettingEditor(SerializedObject serializedMaxSetting)
        {
            this.serializedMaxSetting = serializedMaxSetting;
            propertySdkKey = serializedMaxSetting.FindProperty("sdkKey");
            propertyUserId = serializedMaxSetting.FindProperty("userId");
            propertyUserSegment = serializedMaxSetting.FindProperty("userSegment");
            SettingObject_Load();
        }
        #endregion

        #region Method
        public void OnInspectorGUI()
        {
            bool updateSdk = false;
            serializedMaxSetting.Update();
            //
            GUILayout.BeginVertical("MaxMediationSetting", "window");
            if (serializedMaxMediation == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Create Max Mediation Setting"))
                {
                    SettingObject_Create();
                    SettingObject_Load();
                }
                GUILayout.EndHorizontal();
            }
            if (serializedMaxMediation != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(propertySdkKey, new GUIContent("Sdk Key"));
                updateSdk = EditorGUI.EndChangeCheck();
            }
            EditorGUILayout.PropertyField(propertyUserId, new GUIContent("User Id"));
            EditorGUILayout.PropertyField(propertyUserSegment, new GUIContent("User Segment"));
            GUILayout.EndVertical();
            //
            serializedMaxSetting.ApplyModifiedProperties();
            if (updateSdk)
            {
                serializedMaxMediation.Update();
                propertyMaxMediationSdkKey.stringValue = propertySdkKey.stringValue;
                serializedMaxMediation.ApplyModifiedProperties();
            }
        }
        #endregion

        #region MaxMediation
        private void SettingObject_Load()
        {
            ScriptableObject scriptable = AssetDatabase.LoadAssetAtPath<ScriptableObject>(ASSET_MAX_SDK_SETTING_PATH);
            if (scriptable == null)
            {
                serializedMaxMediation = null;
                propertyMaxMediationSdkKey = null;
                return;
            }
            serializedMaxMediation = new SerializedObject(scriptable);
            propertyMaxMediationSdkKey = serializedMaxMediation.FindProperty("sdkKey");
        }
        private void SettingObject_Create()
        {
            if (!AssetFinder.Exists(ASSET_MAX_SDK_SETTING_FOLDER_NAME))
            {
                AssetFinder.CreateFolder(ASSET_MAX_SDK_SETTING_FOLDER_NAME);
                AssetDatabase.Refresh();
            }
            ScriptableObject scriptable = ScriptableObject.CreateInstance("AppLovinSettings");
            AssetDatabase.CreateAsset(scriptable, ASSET_MAX_SDK_SETTING_PATH);
        }
        #endregion
    }
}
