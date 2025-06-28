using KTool.FileIo;
using UnityEditor;
using UnityEngine;

namespace KTool.Editor
{
    public static class MenuItemTime
    {
        #region Properties

        #endregion Properties

        #region Method
        [MenuItem(itemName: "KTool/Time/TimeScale 0", priority = 0)]
        public static void Data_TimeScale_0()
        {
            Time.timeScale = 0;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 0.1", priority = 1)]
        public static void Data_TimeScale_0_1()
        {
            Time.timeScale = 0.1f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 0.2", priority = 2)]
        public static void Data_TimeScale_0_2()
        {
            Time.timeScale = 0.2f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 0.5", priority = 3)]
        public static void Data_TimeScale_0_5()
        {
            Time.timeScale = 0.5f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 0.75", priority = 4)]
        public static void Data_TimeScale_0_75()
        {
            Time.timeScale = 0.75f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 1", priority = 5)]
        public static void Data_TimeScale_1()
        {
            Time.timeScale = 1f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 2", priority = 6)]
        public static void Data_TimeScale_2()
        {
            Time.timeScale = 2f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 5", priority = 7)]
        public static void Data_TimeScale_5()
        {
            Time.timeScale = 5f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 10", priority = 8)]
        public static void Data_TimeScale_10()
        {
            Time.timeScale = 10f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 20", priority = 9)]
        public static void Data_TimeScale_20()
        {
            Time.timeScale = 20f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 50", priority = 10)]
        public static void Data_TimeScale_50()
        {
            Time.timeScale = 50f;
        }
        [MenuItem(itemName: "KTool/Time/TimeScale 100", priority = 11)]
        public static void Data_TimeScale_100()
        {
            Time.timeScale = 100f;
        }
        #endregion Method
    }
}
