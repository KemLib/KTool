using UnityEngine;
using UnityEditor;

namespace KTool.Loading.Editor
{
    public class CreateGameObject
    {
        #region Properties
        private const string GAME_OBJECT_NAME = "KTool_LoadManager";
        #endregion

        #region Methods
        [MenuItem("GameObject/KTool/Create LoadManager")]
        private static void MenuItem_CreateManager()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME);
            newGO.AddComponent<LoadManager>();
        }
        #endregion
    }
}
