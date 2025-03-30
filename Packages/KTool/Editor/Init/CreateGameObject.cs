﻿using UnityEditor;
using UnityEngine;

namespace KTool.Init.Editor
{
    public class CreateGameObject
    {
        #region Properties
        private const string GAME_OBJECT_NAME_INIT_MANAGER = "KTool_Init_Manager",
            GAME_OBJECT_NAME_INIT_CONTAINER = "KTool_Init_Container";
        #endregion

        #region Methods
        [MenuItem("GameObject/KTool/Create Init Manager")]
        private static void Create_InitManager()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME_INIT_MANAGER);
            newGO.AddComponent<InitManager>();
        }
        [MenuItem("GameObject/KTool/Create Init Container")]
        private static void Create_InitContainer()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME_INIT_CONTAINER);
            newGO.AddComponent<InitContainer>();
        }
        #endregion
    }
}
