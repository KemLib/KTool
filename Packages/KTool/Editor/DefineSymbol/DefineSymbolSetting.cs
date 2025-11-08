using KTool.FileIo;
using UnityEditor;
using UnityEngine;

namespace KTool.DefineSymbol.Editor
{
    public class DefineSymbolSetting : ScriptableObject
    {
        #region Properties
        private const string PACKAGE_INSTANCE_PATH = "Packages/com.kem.ktool/Editor/DefineSymbol/SettingDefineSymbol.asset",
            ASSET_INSTANCE_FOLDER = "Assets/KTool/DefineSymbol/Editor",
            ASSET_INSTANCE_FILE_NAME = "SettingDefineSymbol",
            ASSET_INSTANCE_PATH = "Assets/KTool/DefineSymbol/Editor/SettingDefineSymbol.asset";
        private const string ERROR_CREATE_ASSET_FAIL = "Failed to copy asset SettingDefineSymbol to Resources folder";

        [SerializeField]
        private string[] defineSymbols;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods
        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (AssetFinder.Exists(ASSET_INSTANCE_FOLDER, ASSET_INSTANCE_FILE_NAME, ExtensionType.ASSET))
                return;
            AssetFinder.CreateFolder(ASSET_INSTANCE_FOLDER);
            if (!AssetFinder.Clone(PACKAGE_INSTANCE_PATH, ASSET_INSTANCE_PATH, true))
            {
                Debug.LogError(ERROR_CREATE_ASSET_FAIL);
            }
        }
        #endregion
    }
}
