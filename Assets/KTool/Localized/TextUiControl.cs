using KTool.Attribute;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.Localized
{
    [RequireComponent(typeof(Text))]
    public class TextUiControl : TextControl
    {
        #region Properties
        [SerializeField]
        [GetComponentAttribute]
        private Text textUi;
        [SerializeField]
        private FormatType format;

        private TextItem textItem;
        #endregion Properties

        #region Unity Event

        #endregion Unity Event

        #region Method
        public override void OnInit()
        {
            textItem = new TextItem(textUi, format);
            textItem.Init();
        }
        public override void OnChangeLanguage()
        {
            textItem.ChangeLanguage();
        }
        #endregion Method
    }
}
