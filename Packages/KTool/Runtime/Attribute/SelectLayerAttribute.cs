using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SelectLayerAttribute : PropertyAttribute
    {
        #region Properties

        #endregion Properties

        #region Constructor
        public SelectLayerAttribute() : base()
        {

        }
        #endregion Constructor

        #region Method

        #endregion Method
    }
}
