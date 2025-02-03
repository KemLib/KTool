using System.Text;

namespace KTool.Localized
{
    public static class FormatUnit
    {
        #region Properties
        private const char CHAR_SPACE = ' ';
        #endregion

        #region Method
        public static string GetValue_Format(string value, FormatType format)
        {
            switch (format)
            {
                case FormatType.Lower:
                    return GetValue_Lower(value);
                case FormatType.Upper:
                    return GetValue_Upper(value);
                case FormatType.Sentence:
                    return GetValue_Sentence(value);
                case FormatType.Title:
                    return GetValue_Title(value);
                default:
                    return value;
            }
        }
        public static string GetValue_Lower(string value)
        {
            return value.ToLower();
        }
        public static string GetValue_Upper(string value)
        {
            return value.ToUpper();
        }
        public static string GetValue_Sentence(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            if (value.Length == 1)
                return value.ToUpper();
            //
            string first = value.Substring(0, 1),
                end = value.Substring(1);
            return first.ToUpper() + end;
        }
        public static string GetValue_Title(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            if (value.Length == 1)
                return value.ToUpper();
            //
            StringBuilder stringBuilder = new StringBuilder();
            string[] units = value.Split(CHAR_SPACE);
            int maxIndex = units.Length - 1;
            for (int i = 0; i < units.Length; i++)
            {
                stringBuilder.Append(GetValue_Sentence(units[i]));
                if (i < maxIndex)
                    stringBuilder.Append(CHAR_SPACE);
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}
