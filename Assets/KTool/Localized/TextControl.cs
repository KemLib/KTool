using KTool.Localized.ThirdParty;
using UnityEngine;

namespace KTool.Localized
{
    public abstract class TextControl : MonoBehaviour
    {
        #region Properties

        #endregion Properties

        #region Method
        public abstract void OnInit();
        public abstract void OnChangeLanguage();
        #endregion Method

        #region Value
        public static string GetValue(string defaultValue, FormatType format)
        {
            string value = GetValue(defaultValue);
            return FormatUnit.GetValue_Format(value, format);
        }
        public static string GetValue(string defaultValue, FormatType format, TsvTable table, TsvRow row)
        {
            string value = GetValue(defaultValue, table, row);
            return FormatUnit.GetValue_Format(value, format);
        }
        public static string GetValue(string defaultValue)
        {
            TsvTable dataTable;
            TsvRow dataRow;
            LocalizedManager.Instance.Language_GetRow(defaultValue, out dataTable, out dataRow);
            if (dataTable == null || dataRow == null)
                return defaultValue;
            return GetValue(defaultValue, dataTable, dataRow);
        }
        public static string GetValue(string defaultValue, TsvTable table, TsvRow row)
        {
            if (table == null || row == null)
                return defaultValue;
            //
            int indexColumeCurrent = table.Colume_GetIndex(LocalizedManager.Instance.StringCurrentLanguage);
            if (indexColumeCurrent >= 0)
                return row[indexColumeCurrent];
            //
            int indexColumeDefault = table.Colume_GetIndex(LocalizedManager.Instance.StringDefaultLanguage);
            if (indexColumeDefault >= 0)
                return row[indexColumeDefault];
            //
            return defaultValue;
        }
        #endregion
    }
}
