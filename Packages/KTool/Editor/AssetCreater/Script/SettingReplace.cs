using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{

    public abstract class SettingReplace : ScriptableObject
    {
        #region Properties

        #endregion

        #region Methods
        public abstract Replace GetReplace(CreaterScript createrScript);
        #endregion
    }
}
