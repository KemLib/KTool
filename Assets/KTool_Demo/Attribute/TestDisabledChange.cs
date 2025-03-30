using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestDisabledChange : MonoBehaviour
    {
        #region Properties
        [SerializeField, DisabledChange(false)]
        private string stringVaue;
        [SerializeField, DisabledChange(false)]
        private string[] stringVaues;
        [SerializeField, DisabledChange, Range(1, 100)]
        private int intVaue;
        [SerializeField, DisabledChange]
        private int[] intVaues;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
