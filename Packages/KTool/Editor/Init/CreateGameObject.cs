using UnityEngine;
using UnityEditor;

namespace KTool.Init.Editor
{
    public class CreateGameObject
    {
        #region Properties
        private const string GAME_OBJECT_NAME = "KTool_InitManager";
        #endregion

        #region Methods
        [MenuItem("GameObject/KTool/Create InitManager")]
        private static void MenuItem_CreateManager()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME);
            newGO.AddComponent<InitManager>();
        }
        #endregion
    }
}
