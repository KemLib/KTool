using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestSelectLayer : MonoBehaviour
    {
        #region Properties
        [SerializeField, SelectLayer]
        private int layerId;
        [SerializeField, SelectLayer]
        private int[] layerIds;
        [SerializeField, SelectLayer]
        private string layerName;
        [SerializeField, SelectLayer]
        private string[] layerNames;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
