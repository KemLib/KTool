﻿using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestGetComponentInChildren : MonoBehaviour
    {
        #region Properties
        [SerializeField, GetComponent(GetComponentType.InChildren)]
        private Collider2D collider2d;
        [SerializeField, GetComponent(GetComponentType.InChildren)]
        private Collider2D[] collider2ds;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
