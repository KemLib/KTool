using KTool.FileIo;
using UnityEditor;
using UnityEngine;

namespace Packages.KTool.Editor.AssetCreater.Script
{
    public class SettingCreateScript : ScriptableObject
    {
        #region Poperties
        private const string PATH_INSTANCE_PACKAGE = "Packages/com.kem.ktool/Editor/AssetCreater/Script/SettingCreateScript.asset",
            FOLDER_INSTANCE_ASSET = "Assets/KTool/AssetCreater/Editor/Script",
            PATH_INSTANCE_ASSET = "Assets/KTool/AssetCreater/Editor/Script/SettingCreateScript.asset";
        private const string ERROR_CREATE_ASSET_FAIL = "Failed to copy asset SettingUsingNamespace to Resources folder";

        private static SettingCreateScript instanceDefault;
        private static SettingCreateScript instance;
        private static SettingCreateScript InstancePackage
        {
            get
            {
                if (instanceDefault == null)
                    instanceDefault = AssetDatabase.LoadAssetAtPath<SettingCreateScript>(PATH_INSTANCE_PACKAGE);
                return instanceDefault;
            }
        }
        public static SettingCreateScript Instance
        {
            get
            {
                if (instance == null)
                    instance = AssetDatabase.LoadAssetAtPath<SettingCreateScript>(PATH_INSTANCE_ASSET);
                return instance;
            }
        }

        [SerializeField]
        private SettingTemplate[] templates;

        public int Count => templates.Length;
        public SettingTemplate this[int index] => templates[index];
        #endregion

        #region Methods Unity
        private void OnValidate()
        {
            foreach (var template in templates)
                template.OnValidate();
        }
        #endregion

        #region Methods

        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (AssetDatabase.AssetPathExists(PATH_INSTANCE_ASSET))
                return;
            AssetFinder.CreateFolder(FOLDER_INSTANCE_ASSET);
            if (!AssetDatabase.CopyAsset(PATH_INSTANCE_PACKAGE, PATH_INSTANCE_ASSET))
                Debug.LogError(ERROR_CREATE_ASSET_FAIL);
        }
        #endregion
    }
}
