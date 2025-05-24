using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestSelectComponent : MonoBehaviour
    {
        #region Properties
        [SerializeField, SelectComponent(allowNull: true)]
        private Collider2D collider2d;
        [SerializeField, SelectComponent(allowNull: true)]
        private Collider2D[] collider2ds;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
