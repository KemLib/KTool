using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTool
{
    public interface IEntri
    {
        #region Progperties
        public string Name
        {
            get;
        }
        public float Progress
        {
            get;
        }
        public bool IsDone
        { 
            get; 
        }
        #endregion
    }
}
