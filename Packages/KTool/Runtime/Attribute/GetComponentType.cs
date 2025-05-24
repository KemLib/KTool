using UnityEngine;

namespace KTool.Attribute
{
    public enum GetComponentType
    {
        /// <summary>
        /// Get Component in curent GameObject
        /// </summary>
        InGameObject,
        /// <summary>
        /// Get Component in first Children
        /// </summary>
        InChildren,
        /// <summary>
        /// Get Component in all Children
        /// </summary>
        InAllChildren,
        /// <summary>
        /// Get Component in curent GameObject and first Children
        /// </summary>
        InGameObject_Children,
        /// <summary>
        /// Get Component in curent GameObject and all Children
        /// </summary>
        InGameObject_AllChildren,
    }
}
