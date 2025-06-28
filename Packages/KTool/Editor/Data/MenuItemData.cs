using KTool.FileIo;
using UnityEditor;
using UnityEngine;

namespace KTool.Data.Editor
{
    public static class MenuItemData
    {
        #region Properties

        #endregion Properties

        #region Method
        [MenuItem(itemName: "KTool/Data/Open Data folder")]
        public static void Data_OpenDataFolder()
        {
            try
            {
                System.Diagnostics.Process.Start(PathUnit.PathFolderData);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        [MenuItem(itemName: "KTool/Data/Clear PlayerPrefs")]
        public static void Data_Clear()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem(itemName: "KTool/Data/Clear Data Folder")]
        public static void Data_ClearDataFolder()
        {
            DataFinder.Delete(true);
        }

        [MenuItem(itemName: "KTool/Data/Clear All")]
        public static void Data_ClearAll()
        {
            Data_Clear();
            Data_ClearDataFolder();
        }
        #endregion Method
    }
}
