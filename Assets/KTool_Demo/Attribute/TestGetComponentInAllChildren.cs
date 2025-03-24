using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestGetComponentInAllChildren : MonoBehaviour
    {
        #region Properties
        [SerializeField, GetComponent(GetComponentType.InAllChildren)]
        private Collider2D collider2d;
        [SerializeField, GetComponent(GetComponentType.InAllChildren)]
        private Collider2D[] collider2ds;
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
