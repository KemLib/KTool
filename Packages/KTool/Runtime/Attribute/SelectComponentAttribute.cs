using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SelectComponentAttribute : PropertyAttribute
    {
        #region Properties
        private GetComponentType getComponentType;
        private bool includeInactive;

        public GetComponentType GetComponentType => getComponentType;
        public bool IncludeInactive => includeInactive;
        #endregion

        #region Constructor
        public SelectComponentAttribute(GetComponentType getComponentType = GetComponentType.ThisGameObject, bool includeInactive = false) : base()
        {
            this.getComponentType = getComponentType;
            this.includeInactive = includeInactive;
        }
        #endregion

        #region Methods

        #endregion
    }
}
