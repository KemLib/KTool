using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestSelectSortingLayer : MonoBehaviour
    {
        #region Properties
        [SerializeField, SelectSortingLayer]
        private int sortingLayerId;
        [SerializeField, SelectSortingLayer]
        private int[] sortingLayerIds;
        [SerializeField, SelectSortingLayer]
        private string sortingLayerName;
        [SerializeField, SelectSortingLayer]
        private string[] sortingLayerNames;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
