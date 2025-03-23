using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace KTool.Attribute
{
    public static class AttributeUnit
    {
        #region Properties

        #endregion

        #region Method
        public static int Agent_GetIndex(int id)
        {
            int count = NavMesh.GetSettingsCount();
            for (int i = 0; i < count; i++)
                if (NavMesh.GetSettingsByIndex(i).agentTypeID == id)
                    return i;
            return -1;
        }
        public static int Agent_GetId(int index)
        {
            return NavMesh.GetSettingsByIndex(index).agentTypeID;
        }

        public static string[] Array_Agent()
        {
            string[] agents;
            agents = new string[NavMesh.GetSettingsCount()];
            for (int i = 0; i < agents.Length; i++)
            {
                NavMeshBuildSettings navSetting = NavMesh.GetSettingsByIndex(i);
                agents[i] = NavMesh.GetSettingsNameFromID(navSetting.agentTypeID);
            }
            return agents;
        }
        public static string[] Array_Layer()
        {
            List<string> layers = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                string name = LayerMask.LayerToName(i);
                if (string.IsNullOrEmpty(name))
                    continue;
                layers.Add(name);
            }
            return layers.ToArray();
        }
        public static string[] Array_Scene()
        {
            string[] scenes;
#if UNITY_EDITOR
            scenes = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
            }
#else
            scenes = new string[0];
#endif
            return scenes;
        }
        public static string[] Array_SortingLayer()
        {
            string[] layers = new string[SortingLayer.layers.Length];
            for (int i = 0; i < layers.Length; i++)
                layers[i] = SortingLayer.layers[i].name;
            return layers;
        }
        public static string[] Array_Tag()
        {
            string[] tags;
#if UNITY_EDITOR
            tags = new string[InternalEditorUtility.tags.Length];
            for (int i = 0; i < tags.Length; i++)
                tags[i] = InternalEditorUtility.tags[i];
#else
            tags = new string[0];
#endif
            return tags;
        }
        #endregion
    }
}
