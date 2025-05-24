using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class GetComponentAttribute : PropertyAttribute
    {
        #region Properties
        private GetComponentType getComponentType;
        private bool allowInactive;

        public GetComponentType GetComponentType => getComponentType;
        public bool AllowInactive => allowInactive;
        #endregion Properties

        #region Constructor
        public GetComponentAttribute(GetComponentType getComponentType = GetComponentType.InGameObject, bool allowInactive = false) : base()
        {
            this.getComponentType = getComponentType;
            this.allowInactive = allowInactive;
        }
        #endregion Constructor

        #region Method

        #endregion Method
    }
}
