using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class GetComponentAttribute : PropertyAttribute
    {
        #region Properties
        private GetComponentType getComponentType;
        private bool includeInactive;

        public GetComponentType GetComponentType => getComponentType;
        public bool IncludeInactive => includeInactive;
        #endregion Properties

        #region Constructor
        public GetComponentAttribute(GetComponentType getComponentType = GetComponentType.InGameObject, bool includeInactive = false) : base()
        {
            this.getComponentType = getComponentType;
            this.includeInactive = includeInactive;
        }
        #endregion Constructor

        #region Method

        #endregion Method
    }
}
