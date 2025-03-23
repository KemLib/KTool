using UnityEngine;
using UnityEngine.UI;

namespace KTool.MenuAnim
{
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(Canvas))]
    public class CanvasScaleFixMatch : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private bool autoUpdate_CanvasSize;
        [SerializeField]
        private CanvasTemplate[] canvasTemplates;

        private Canvas canvas;
        private CanvasScaler canvasScaler;
        private Vector2 canvasSize = Vector2.zero;

        public int Count => canvasTemplates.Length;
        public CanvasTemplate this[int index] => canvasTemplates[index];
        #endregion

        #region Unity Event
        private void Awake()
        {
            Init();
        }

        private void LateUpdate()
        {
            Update_CanvasSize();
        }
        #endregion

        #region Method
        private void Init()
        {
            canvas = GetComponent<Canvas>();
            canvasScaler = GetComponent<CanvasScaler>();
            CanvasTemplate.Sort(canvasTemplates);
            canvasSize = canvas.pixelRect.size;
            Update_CanvasTemplate(canvasSize);
        }

        private void Update_CanvasSize()
        {
            if (!autoUpdate_CanvasSize || canvasTemplates == null || canvasTemplates.Length <= 0)
                return;
            Vector2 newCanvasSize = canvas.pixelRect.size;
            if (canvasSize.x != newCanvasSize.x || canvasSize.y != newCanvasSize.y)
            {
                canvasSize = newCanvasSize;
                Update_CanvasTemplate(canvasSize);
            }
        }
        public void Update_CanvasTemplate(Vector2 screenSize)
        {
            int indexSelect = CanvasTemplate.GetIndex(canvasTemplates, screenSize);
            if (indexSelect == -1)
                CanvasMatch_SetDefault(screenSize);
            else
                CanvasMatch_Set(canvasTemplates[indexSelect]);
        }
        private void CanvasMatch_Set(CanvasTemplate factor)
        {
            canvasScaler.matchWidthOrHeight = factor.Match;
        }
        private void CanvasMatch_SetDefault(Vector2 screenSize)
        {
            CanvasTemplate factor = new CanvasTemplate(screenSize.x, screenSize.y, canvasScaler.matchWidthOrHeight);
            //
            CanvasMatch_Set(factor);
        }
        #endregion
    }
}
