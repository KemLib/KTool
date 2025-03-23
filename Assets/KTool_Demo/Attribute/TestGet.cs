using UnityEngine;
using KTool.Attribute;

namespace KTool_Demo.Attribute
{
    public class TestGet : MonoBehaviour
    {
        #region Properties
        [SerializeField, GetAssetInFolder("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private TextAsset[] textAssets;
        [SerializeField, GetFileNameInFolder("Packages/com.kem.ktool/Editor/AssetCreater/Script/Templates", KTool.FileIo.ExtensionType.TXT)]
        private string[] filenames;
        [SerializeField, GetComponent]
        private Collider2D collider2d;
        [SerializeField, GetComponent]
        private Collider2D[] collider2ds;
        [SerializeField, GetComponentInChildren]
        private Collider2D collider2dInChildren;
        [SerializeField, GetComponentInChildren(true)]
        private Collider2D[] collider2dsInChildren;
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
