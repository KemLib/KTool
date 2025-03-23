using UnityEngine;
using UnityEditor;

namespace KTool.Advertisement.Editor
{
    public class CreateGameObject
    {
        #region Properties
        private const string GAME_OBJECT_NAME = "KTool_AdvertisementManager";
        #endregion

        #region Methods
        [MenuItem("GameObject/KTool/Create AdvertisementManager")]
        private static void MenuItem_CreateManager()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME);
            newGO.AddComponent<AdvertisementManager>();
        }
        #endregion
    }
}
