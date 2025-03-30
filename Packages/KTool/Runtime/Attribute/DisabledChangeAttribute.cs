using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DisabledChangeAttribute : PropertyAttribute
    {
        #region Properties
        private bool enableChange;

        public bool EnableChange => enableChange;
        #endregion

        #region Construction
        public DisabledChangeAttribute(bool enableChange = true)
        {
            this.enableChange = enableChange;
        }
        #endregion

        #region Methods

        #endregion
    }
}
