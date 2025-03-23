using UnityEngine;
using KTool.Attribute;

namespace KTool_Demo.Attribute
{
    public class TestSelect : MonoBehaviour
    {
        #region Properties
        [SerializeField, SelectAgentId]
        private int agentId;
        [SerializeField, SelectAgentId]
        private int[] agentIds;
        [SerializeField, SelectAssetInFolder("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset assetInFolder;
        [SerializeField, SelectAssetInFolder("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset[] assetInFolders;
        [SerializeField, SelectFileNameInFolder("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private string filenameInFolder;
        [SerializeField, SelectFileNameInFolder("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private string[] filenameInFolders;
        [SerializeField, SelectLayer]
        private int layerId;
        [SerializeField, SelectLayer]
        private int[] layerIds;
        [SerializeField, SelectLayer]
        private string layerName;
        [SerializeField, SelectLayer]
        private string[] layerNames;
        [SerializeField, SelectScene]
        private int sceneId;
        [SerializeField, SelectScene]
        private int[] sceneIds;
        [SerializeField, SelectScene]
        private string sceneName;
        [SerializeField, SelectScene]
        private string[] sceneNames;
        [SerializeField, SelectSortingLayer]
        private int sortingLayerId;
        [SerializeField, SelectSortingLayer]
        private int[] sortingLayerIds;
        [SerializeField, SelectSortingLayer]
        private string sortingLayerName;
        [SerializeField, SelectSortingLayer]
        private string[] sortingLayerNames;
        [SerializeField, SelectTag]
        private int tagId;
        [SerializeField, SelectTag]
        private int[] tagIds;
        [SerializeField, SelectTag]
        private string tagName;
        [SerializeField, SelectTag]
        private string[] tagNames;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
