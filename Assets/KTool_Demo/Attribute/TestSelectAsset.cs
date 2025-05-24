using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestSelectAsset : MonoBehaviour
    {
        #region Properties
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT, true)]
        private TextAsset assetInFolder_1;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates/Null", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset assetInFolder_2;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset assetInFolder_3;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset[] assetInFolders_1;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates/Null", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset[] assetInFolders_2;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset[] assetInFolders_3;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private string assetNameInFolder_1;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates/Null", KTool.FileIo.ExtensionType.TXT)]
        private string assetNameInFolder_2;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script", KTool.FileIo.ExtensionType.TXT)]
        private string assetNameInFolder_3;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private string[] assetNameInFolders_1;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates/Null", KTool.FileIo.ExtensionType.TXT)]
        private string[] assetNameInFolders_2;
        [SerializeField, SelectAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script", KTool.FileIo.ExtensionType.TXT)]
        private string[] assetNameInFolders_3;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
