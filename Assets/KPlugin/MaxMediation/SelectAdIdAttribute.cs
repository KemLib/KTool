using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KPlugin.MaxMediation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SelectAdIdAttribute : PropertyAttribute
    {
        #region Properties
        private MaxAdType type;

        public MaxAdType Type => type;
        #endregion

        #region Construction
        public SelectAdIdAttribute(MaxAdType type) : base()
        {
            this.type = type;
        }
        #endregion

        #region Method

        #endregion
    }
}