using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestSelectTag : MonoBehaviour
    {
        #region Properties
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
