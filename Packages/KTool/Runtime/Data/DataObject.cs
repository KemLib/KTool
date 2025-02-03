using KTool.ThirdParty;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace KTool.Data
{
    public abstract class DataObject
    {
        #region Properties
        private const string FORMAT_KEY_CONTENT = "{0}.{1}";
        private const string TIME_CULTURE = "en-US";

        private static CultureInfo DateTimeCultureInfo = new CultureInfo(TIME_CULTURE);

        private string keyRoot;
        #endregion Properties

        #region Construction
        public DataObject(string keyRoot)
        {
            this.keyRoot = keyRoot;
        }

        #endregion Construction

        #region Key
        protected string Key_Get(string keyContent)
        {
            return string.Format(FORMAT_KEY_CONTENT, keyRoot, keyContent);
        }
        #endregion Key

        #region Convert
        public static string ConvertToString(DateTime dateTime)
        {
            return dateTime.ToString(DateTimeCultureInfo);
        }
        public static string ConvertToString(Dictionary<string, object> dic)
        {
            return Json.Serialize(dic);
        }
        public static string ConvertToString(List<object> list)
        {
            return Json.Serialize(list);
        }
        public static DateTime ConvertToDate(string time)
        {
            return Convert.ToDateTime(time, DateTimeCultureInfo);
        }
        public static Dictionary<string, object> ConvertToDictionary(string jsonData)
        {
            return Json.Deserialize(jsonData) as Dictionary<string, object>;
        }
        public static List<object> ConvertToList(string jsonData)
        {
            return Json.Deserialize(jsonData) as List<object>;
        }
        #endregion Convert
    }
}
