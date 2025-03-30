using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestGet : MonoBehaviour
    {
        #region Properties
        [SerializeField, GetAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset[] textAssets_1;
        [SerializeField, GetAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates/Null", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset[] textAssets_2;
        [SerializeField, GetAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset[] textAssets_3;
        [SerializeField, GetAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private string[] textAssetsName_1;
        [SerializeField, GetAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates/Null", KTool.FileIo.ExtensionType.TXT)]
        private string[] textAssetsName_2;
        [SerializeField, GetAsset("Packages/com.kem.ktool/Editor/AssetCreater/Script", KTool.FileIo.ExtensionType.TXT)]
        private string[] textAssetsName_3;
        #endregion

        #region Methods Unity
        private void Start()
        {

        }
        private void Update()
        {

        }
        #endregion

        #region Methods

        #endregion
    }
}
