using UnityEditor;
using UnityEngine;

namespace KPlugin.AdMob.Editor
{
    [CustomEditor(typeof(AdMobAdAppResume))]
    public class AdMobAdResumeEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertyInitType,
            propertyAdTagetType,
            propertyAdAppOpen,
            propertyAdInterstitial,
            propertyAdRewardedInterstitial,
            propertyIsActive;
        #endregion

        #region Unity Event
        private void OnEnable()
        {
            Init();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //
            EditorGUILayout.PropertyField(propertyInitType, new GUIContent("Init Type"));
            EditorGUILayout.PropertyField(propertyAdTagetType, new GUIContent("Ad Taget Type"));
            AdMobAdAppResume.AdTagetType adType = (AdMobAdAppResume.AdTagetType)propertyAdTagetType.enumValueIndex;
            switch (adType)
            {
                case AdMobAdAppResume.AdTagetType.AdAppOpen:
                    EditorGUILayout.PropertyField(propertyAdAppOpen, new GUIContent("Ad Taget"));
                    break;
                case AdMobAdAppResume.AdTagetType.AdInterstitial:
                    EditorGUILayout.PropertyField(propertyAdInterstitial, new GUIContent("Ad Taget"));
                    break;
                case AdMobAdAppResume.AdTagetType.AdRewardedInterstitial:
                    EditorGUILayout.PropertyField(propertyAdRewardedInterstitial, new GUIContent("Ad Taget"));
                    break;
            }
            EditorGUILayout.PropertyField(propertyIsActive, new GUIContent("Is Active"));
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Method
        private void Init()
        {
            propertyInitType = serializedObject.FindProperty("initType");
            propertyAdTagetType = serializedObject.FindProperty("adTagetType");
            propertyAdAppOpen = serializedObject.FindProperty("adAppOpen");
            propertyAdInterstitial = serializedObject.FindProperty("adInterstitial");
            propertyAdRewardedInterstitial = serializedObject.FindProperty("adRewardedInterstitial");
            propertyIsActive = serializedObject.FindProperty("isActive");
        }
        #endregion
    }
}
