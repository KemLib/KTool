using UnityEditor;
using UnityEngine;

namespace KTool.Advertisement.Editor
{
    public class CreateGameObject
    {
        #region Properties
        private const string GAME_OBJECT_NAME = "KTool_Advertisement_Manager";
        #endregion

        #region Methods
        [MenuItem("GameObject/KTool/Create Advertisement Manager")]
        private static void Create_Advertisement_Manager()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME);
            newGO.AddComponent<AdvertisementManager>();
        }
        #endregion
    }
}
