using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.MenuAnim.Editor
{
    [CustomEditor(typeof(CanvasScaleFixMatch))]
    public class CanvasScaleFixMatchEditor : UnityEditor.Editor
    {
        #region Properties
        private CanvasScaleFixMatch tagetObject;
        private SerializedProperty propertyAutoUpdate_CanvasSize;
        private SerializedProperty propertyCanvasTemplates;
        private bool isShow = false;
        private Canvas canvas;
        private CanvasScaler canvasScaler;
        #endregion

        #region Unity Event
        private void OnEnable()
        {
            Init();
        }
        public override void OnInspectorGUI()
        {
            CanvasScaler_FixMode();
            //
            serializedObject.Update();
            EditorGUILayout.PropertyField(propertyAutoUpdate_CanvasSize, new GUIContent("Auto Update Screen Size"));
            OnInspector_ScreenTemplatesArray();
            CanvasScaler_FixMatch();
            serializedObject.ApplyModifiedProperties();
        }
        private void OnInspector_ScreenTemplatesArray()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            propertyCanvasTemplates.arraySize = EditorGUILayout.IntField(new GUIContent("Canvas Templates Size"), propertyCanvasTemplates.arraySize);
            if (propertyCanvasTemplates.arraySize > 0)
            {
                if (isShow)
                {
                    if (GUILayout.Button("Hide CanvasTemplates"))
                        isShow = false;
                }
                else
                {
                    if (GUILayout.Button("Show CanvasTemplates"))
                        isShow = true;
                }
            }
            GUILayout.EndHorizontal();
            //
            if (propertyCanvasTemplates.arraySize > 0 && isShow)
            {
                int index = 0;
                foreach (SerializedProperty propertyScreenTemplate in propertyCanvasTemplates)
                {
                    GUILayout.Space(5);
                    OnInspector_ScreenTemplate("CanvasTemplates " + index, propertyScreenTemplate);
                    index++;
                }
            }
        }
        private void OnInspector_ScreenTemplate(string title, SerializedProperty propertyScreenTemplate)
        {
            GUILayout.BeginVertical(title, "window");
            //
            SerializedProperty propertyScreenWidth = propertyScreenTemplate.FindPropertyRelative("width");
            SerializedProperty propertyScreenHeight = propertyScreenTemplate.FindPropertyRelative("hight");
            SerializedProperty propertyCanvasScalerMatch = propertyScreenTemplate.FindPropertyRelative("match");
            //
            EditorGUILayout.PropertyField(propertyScreenWidth, new GUIContent("Width"));
            EditorGUILayout.PropertyField(propertyScreenHeight, new GUIContent("Height"));
            EditorGUILayout.PropertyField(propertyCanvasScalerMatch, new GUIContent("Match"));
            //
            GUILayout.EndVertical();
        }
        #endregion Unity Event

        #region Method
        private void Init()
        {
            tagetObject = serializedObject.targetObject as CanvasScaleFixMatch;
            canvas = tagetObject.GetComponent<Canvas>();
            canvasScaler = tagetObject.GetComponent<CanvasScaler>();
            propertyAutoUpdate_CanvasSize = serializedObject.FindProperty("autoUpdate_CanvasSize");
            propertyCanvasTemplates = serializedObject.FindProperty("canvasTemplates");
        }
        private void CanvasScaler_FixMode()
        {
            if (canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                EditorUtility.SetDirty(canvasScaler);
            }
            if (canvasScaler.screenMatchMode != CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
            {
                canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                EditorUtility.SetDirty(canvasScaler);
            }
        }
        private void CanvasScaler_FixMatch()
        {
            if (propertyCanvasTemplates.arraySize < 0)
                return;
            //
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Fix Match"))
            {
                CanvasTemplate[] templates = new CanvasTemplate[tagetObject.Count];
                for (int i = 0; i < tagetObject.Count; i++)
                    templates[i] = tagetObject[i];
                CanvasTemplate.Sort(templates);
                int index = CanvasTemplate.GetIndex(templates, canvas.pixelRect.size);
                if (index >= 0)
                {
                    canvasScaler.matchWidthOrHeight = templates[index].Match;
                    EditorUtility.SetDirty(canvasScaler);
                }
            }
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}
