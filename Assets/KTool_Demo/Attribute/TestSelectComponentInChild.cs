using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestSelectComponentInChild : MonoBehaviour
    {
        #region Properties
        [SerializeField, SelectComponent(GetComponentType.InChildren)]
        private Collider2D collider2d;
        [SerializeField, SelectComponent(GetComponentType.InChildren)]
        private Collider2D[] collider2ds;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
