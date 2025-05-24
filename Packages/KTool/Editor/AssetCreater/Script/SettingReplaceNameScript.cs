using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public class SettingReplaceNameScript : SettingReplace
    {
        #region Properties
        public const string KEY_NAME_SCRIPT = "[NameScript]";

        [SerializeField]
        private string keyNameScript = KEY_NAME_SCRIPT;
        [SerializeField]
        private Color colorTextPrivew;
        [SerializeField]
        private bool editFileName;

        public string KeyNameScript => string.IsNullOrEmpty(keyNameScript) ? KEY_NAME_SCRIPT : keyNameScript;
        public Color ColorTextPrivew => colorTextPrivew;
        public bool EditFileName => editFileName;
        #endregion

        #region Methods Unity
        public override Replace GetReplace(CreaterScript createrScript)
        {
            return new ReplaceNameScript(createrScript, this);
        }
        #endregion

        #region Methods

        #endregion
    }
}
