using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class GetComponentAttribute : PropertyAttribute
    {
        #region Properties

        #endregion Properties

        #region Constructor
        public GetComponentAttribute() : base()
        {

        }
        #endregion Constructor

        #region Method

        #endregion Method
    }
}
