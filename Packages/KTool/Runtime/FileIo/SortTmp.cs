using System;
using System.IO;
using System.Text.RegularExpressions;

namespace KTool.FileIo
{
    public static class SortTmp
    {
        #region Properties
        private const string TXT_REGULAR = "[^0-9]";
        #endregion Properties

        #region Method

        public static void SortString(string[] values)
        {
            Array.Sort(values, (a, b) => int.Parse(Regex.Replace(a, TXT_REGULAR, string.Empty)) - int.Parse(Regex.Replace(b, TXT_REGULAR, string.Empty)));
        }
        public static void SortFileInfo(FileInfo[] files)
        {
            Array.Sort(files, (a, b) => int.Parse(Regex.Replace(a.Name, TXT_REGULAR, string.Empty)) - int.Parse(Regex.Replace(b.Name, TXT_REGULAR, string.Empty)));
        }
        public static void SortDirectoryInfo(DirectoryInfo[] directorys)
        {
            Array.Sort(directorys, (a, b) => int.Parse(Regex.Replace(a.Name, TXT_REGULAR, string.Empty)) - int.Parse(Regex.Replace(b.Name, TXT_REGULAR, string.Empty)));
        }
        #endregion Method
    }
}
