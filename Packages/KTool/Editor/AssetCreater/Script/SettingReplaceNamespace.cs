using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    public class SettingReplaceNamespace : SettingReplace
    {
        #region Properties
        public const string KEY_NAMESPACE = "[Namespace]";

        [SerializeField]
        private string keyNamespace = KEY_NAMESPACE;
        [SerializeField]
        private Color colorTextPrivew;

        public string KeyNamespace => string.IsNullOrEmpty(keyNamespace) ? KEY_NAMESPACE : keyNamespace;
        public Color ColorTextPrivew => colorTextPrivew;
        #endregion

        #region Methods Unity
        public override Replace GetReplace(CreaterScript createrScript)
        {
            return new ReplaceNamespace(createrScript, this);
        }
        #endregion

        #region Methods

        #endregion
    }
}
