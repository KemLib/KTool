using KTool.Attribute;
using UnityEngine;

namespace KTool_Demo.Attribute
{
    public class TestSelectAgentId : MonoBehaviour
    {
        #region Properties
        [SerializeField, SelectAgentId]
        private int agentId;
        [SerializeField, SelectAgentId]
        private int[] agentIds;
        #endregion

        #region Methods Unity

        #endregion

        #region Methods

        #endregion
    }
}
