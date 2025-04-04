using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestSelectScene : MonoBehaviour
    {
        #region Properties
        [SerializeField, SelectScene]
        private int sceneId;
        [SerializeField, SelectScene]
        private int[] sceneIds;
        [SerializeField, SelectScene]
        private string sceneName;
        [SerializeField, SelectScene]
        private string[] sceneNames;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
