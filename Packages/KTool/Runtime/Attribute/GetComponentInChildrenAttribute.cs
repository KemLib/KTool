using System;
using UnityEngine;

namespace KTool.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class GetComponentInChildrenAttribute : PropertyAttribute
    {
        #region Properties
        private bool getInTree;

        public bool GetInTree => getInTree;
        #endregion

        #region Constructor
        public GetComponentInChildrenAttribute(bool getInTree = false) : base()
        {
            this.getInTree = getInTree;
        }
        #endregion

        #region Method

        #endregion
    }
}
