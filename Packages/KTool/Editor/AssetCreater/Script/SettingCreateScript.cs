using KTool.FileIo;
using UnityEditor;
using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public class SettingCreateScript : ScriptableObject
    {
        #region Poperties
        private const string PACKAGE_INSTANCE_PATH = "Packages/com.kem.ktool/Editor/AssetCreater/Script/SettingCreateScript.asset",
            ASSET_INSTANCE_FOLDER = "Assets/KTool/AssetCreater/Editor/Script",
            ASSET_INSTANCE_FILE_NAME = "SettingCreateScript",
            ASSET_INSTANCE_PATH = "Assets/KTool/AssetCreater/Editor/Script/SettingCreateScript.asset";
        private const string ERROR_CREATE_ASSET_FAIL = "Failed to copy asset SettingUsingNamespace to Resources folder";

        private static SettingCreateScript instance;

        public static SettingCreateScript Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<SettingCreateScript>(ASSET_INSTANCE_PATH);
                    if (instance == null)
                        instance = AssetDatabase.LoadAssetAtPath<SettingCreateScript>(PACKAGE_INSTANCE_PATH);
                }
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
            if (AssetFinder.Exists(ASSET_INSTANCE_FOLDER, ASSET_INSTANCE_FILE_NAME, ExtensionType.ASSET))
                return;
            AssetFinder.CreateFolder(ASSET_INSTANCE_FOLDER);
            if (!AssetDatabase.CopyAsset(PACKAGE_INSTANCE_PATH, ASSET_INSTANCE_PATH))
                Debug.LogError(ERROR_CREATE_ASSET_FAIL);
        }
        #endregion
    }
}
