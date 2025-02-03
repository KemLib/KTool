using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KTool.FileIo;

namespace KPlugin.MaxMediation.Editor
{
    public static class MenuItemCreateSetting
    {
        #region Properties

        #endregion

        #region Method
        [MenuItem("KPlugin/MaxMediation/Create Setting", priority = 1)]
        private static void MenuItem_CreateMaxSetting()
        {
            MaxSetting scriptable = AssetDatabase.LoadAssetAtPath<MaxSetting>(MaxSettingEditor.ASSET_MAX_SETTING_PATH);
            if (scriptable == null)
                scriptable = CreateMaxSetting();
            //
            Selection.objects = new Object[] { scriptable };
        }

        public static MaxSetting CreateMaxSetting()
        {
            if (!AssetFinder.Exists(MaxSettingEditor.ASSET_MAX_SETTING_FOLDER_NAME))
            {
                AssetFinder.CreateFolder(MaxSettingEditor.ASSET_MAX_SETTING_FOLDER_NAME);
                AssetDatabase.Refresh();
            }
            MaxSetting scriptable = ScriptableObject.CreateInstance<MaxSetting>();
            AssetDatabase.CreateAsset(scriptable, MaxSettingEditor.ASSET_MAX_SETTING_PATH);
            return scriptable;
        }
        #endregion
    }
}
