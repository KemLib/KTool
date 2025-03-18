using KTool.Localized.ThirdParty;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.Localized
{
    [System.Serializable]
    public class TextItem
    {
        [SerializeField]
        private Text text;
        [SerializeField]
        private FormatType format;

        private string defaultValue;
        private Font defaultFont;
        private TsvTable dataTable;
        private TsvRow dataRow;

        public TextItem(Text text, FormatType format)
        {
            this.text = text;
            this.format = format;
        }

        public void Init()
        {
            defaultValue = text.text;
            defaultFont = text.font;
            LocalizedManager.Instance.Language_GetRow(defaultValue, out dataTable, out dataRow);
        }

        public void ChangeLanguage()
        {
            text.text = TextControl.GetValue(defaultValue, format, dataTable, dataRow);
            ChangeFont();
        }

        private void ChangeFont()
        {
            LocalizedData localizedData = (LocalizedManager.Instance == null ? null : LocalizedManager.Instance.CurrentLocalizedData);
            if (localizedData == null || localizedData.Font == null)
                text.font = defaultFont;
            else
                text.font = localizedData.Font;
        }
    }
}
