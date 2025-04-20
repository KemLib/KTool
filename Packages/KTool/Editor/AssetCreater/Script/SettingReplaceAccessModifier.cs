using UnityEngine;

namespace KTool.AssetCreater.Script.Editor
{
    [CreateAssetMenu(fileName = "SettingReplaceAccessModifier", menuName = "ScriptableObject/SettingReplaceAccessModifier")]
    public class SettingReplaceAccessModifier : SettingReplace
    {
        #region Properties
        public const string KEY_ACCESS_MODIFIERS = "[ClassAccessModifiers]";

        [SerializeField]
        private string keyAccessModifiers = KEY_ACCESS_MODIFIERS;
        [SerializeField]
        private Color colorTextPrivew;

        public string KeyAccessModifiers => string.IsNullOrEmpty(keyAccessModifiers) ? KEY_ACCESS_MODIFIERS : keyAccessModifiers;
        public Color ColorTextPrivew => colorTextPrivew;
        #endregion

        #region Methods Unity
        public override Replace GetReplace(CreaterScript createrScript)
        {
            return new ReplaceAccessModifier(createrScript, this);
        }
        #endregion

        #region Methods

        #endregion
    }
}
