using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SelectComponentAttribute : PropertyAttribute
    {
        #region Properties
        private GetComponentType getComponentType;
        private bool allowInactive,
            allowNull;

        public GetComponentType GetComponentType => getComponentType;
        public bool AllowInactive => allowInactive;
        public bool AllowNull => allowNull;
        #endregion

        #region Constructor
        public SelectComponentAttribute(GetComponentType getComponentType = GetComponentType.InGameObject, bool allowInactive = false, bool allowNull = false) : base()
        {
            this.getComponentType = getComponentType;
            this.allowInactive = allowInactive;
            this.allowNull = allowNull;
        }
        #endregion

        #region Methods

        #endregion
    }
}
