using KTool.FileIo;
using UnityEditor;
using UnityEngine;

namespace KPlugin.AdMob.Editor
{
    public static class MenuItemCreateSetting
    {
        #region Properties

        #endregion

        #region Method
        [MenuItem("KPlugin/Google/AdMob/Create Setting", priority = 1)]
        private static void MenuItem_CreateAdMobSetting()
        {
            AdMobSetting scriptable = AssetDatabase.LoadAssetAtPath<AdMobSetting>(AdMobSettingEditor.ASSET_GOOGLE_ADMOB_SETTING_PATH);
            if (scriptable == null)
                scriptable = CreateAdMobSetting();
            //
            Selection.objects = new Object[] { scriptable };
        }

        public static AdMobSetting CreateAdMobSetting()
        {
            if (!AssetFinder.Exists(AdMobSettingEditor.ASSET_ADMOB_SETTING_FOLDER_NAME))
            {
                AssetFinder.CreateFolder(AdMobSettingEditor.ASSET_ADMOB_SETTING_FOLDER_NAME);
                AssetDatabase.Refresh();
            }
            AdMobSetting scriptable = ScriptableObject.CreateInstance<AdMobSetting>();
            AssetDatabase.CreateAsset(scriptable, AdMobSettingEditor.ASSET_GOOGLE_ADMOB_SETTING_PATH);
            return scriptable;
        }
        #endregion
    }
}
