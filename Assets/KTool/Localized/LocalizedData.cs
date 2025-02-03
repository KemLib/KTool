using UnityEngine;

namespace KTool.Localized
{
    [System.Serializable]
    public class LocalizedData
    {
        #region Properties
        [SerializeField]
        private string name;
        [SerializeField]
        private SystemLanguage language;
        [SerializeField]
        private Sprite flag;
        [SerializeField]
        private Font font;

        public string Name => string.IsNullOrEmpty(name) ? Language.ToString() : name;
        public SystemLanguage Language => language;
        public Sprite Flag => flag;
        public Font Font => font;
        #endregion Properties

        #region Method

        #endregion Method
    }
}
