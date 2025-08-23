using System.IO;
using UnityEngine;

namespace KTool.FileIo
{
    public static class PathUnit
    {
        #region Properties
        public const string FOLDER_DATA = "Data",
            FOLDER_ASSET = "Assets";
        private const string FORMAT_FOLDER = "{0}/{1}";
        public const char CHAR_SPECIAL_DOT = '.',
            CHAR_SPECIAL_PATH_WALL = '/';
        public const string FOTMAT_FULL_PATH_FILE = "{0}/{1}",
            FOTMAT_FULL_PATH_FILE_EXTENSION = "{0}/{1}.{2}",
            FORMAT_SEARCH_PATTERN = "*.{0}",
            SEARCH_PATTERN_NONE = "*.*";
        public const string ERROR_FILE_NOT_FOUND = "File not found";


        /// <summary>
        /// [path project]
        /// </summary>
        public static string PathFolderProject
        {
            get
            {
#if UNITY_EDITOR
                DirectoryInfo folder = new DirectoryInfo(Application.dataPath);
                return folder.Parent.FullName;
#elif UNITY_ANDROID || UNITY_IOS
                return string.Empty;
#endif
            }
        }
        /// <summary>
        /// [path project]/Data
        /// </summary>
        public static string PathFolderData
        {
            get
            {
#if UNITY_EDITOR
                return string.Format(FORMAT_FOLDER, PathFolderProject, FOLDER_DATA);
#elif UNITY_ANDROID || UNITY_IOS
                return Application.persistentDataPath;
#endif
            }
        }
        #endregion Properties

        #region Method
        public static string GetFullFolder_Asset(string folder_asset)
        {
            return string.Format(FORMAT_FOLDER, PathFolderProject, folder_asset);
        }
        public static string GetFullFolder_Data(string folder_data)
        {
            return string.Format(FORMAT_FOLDER, PathFolderData, folder_data);
        }

        public static string GetExtension(ExtensionType extension)
        {
            switch (extension)
            {
                case ExtensionType.NONE:
                    return string.Empty;
                case ExtensionType.SCRIPTABLE_OBJECT:
                case ExtensionType.MAT:
                    return ExtensionType.ASSET.ToString().ToLower();
                default:
                    return extension.ToString().ToLower();
            }
        }
        public static string GetExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return string.Empty;
            return extension.ToString().ToLower();
        }
        public static string GetSearchPattern(ExtensionType extension)
        {
            switch (extension)
            {
                case ExtensionType.NONE:
                    return SEARCH_PATTERN_NONE;
                case ExtensionType.SCRIPTABLE_OBJECT:
                case ExtensionType.MAT:
                    return string.Format(FORMAT_SEARCH_PATTERN, GetExtension(ExtensionType.ASSET));
                default:
                    return string.Format(FORMAT_SEARCH_PATTERN, GetExtension(extension));
            }
        }
        public static string GetSearchPattern(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return SEARCH_PATTERN_NONE;
            return string.Format(FORMAT_SEARCH_PATTERN, GetExtension(extension));
        }
        #endregion Method
    }
}
