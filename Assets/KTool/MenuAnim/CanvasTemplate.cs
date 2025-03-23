using System;
using UnityEngine;

namespace KTool.MenuAnim
{
    [Serializable]
    public struct CanvasTemplate
    {
        #region Properties
        [SerializeField]
        [Min(0)]
        private float width,
            hight;
        [SerializeField]
        [Range(0, 1)]
        private float match;

        public float Width => width;
        public float Height => hight;
        public float Aspect => width / hight;
        public float Match => match;
        #endregion

        #region Construction
        public CanvasTemplate(float width, float height, float match)
        {
            this.width = width;
            this.hight = height;
            this.match = match;
        }
        #endregion

        #region Method
        public static int GetIndex(CanvasTemplate[] canvasTemplates, Vector2 canvasSize)
        {
            if (canvasTemplates == null || canvasTemplates.Length == 0)
                return -1;
            //
            int index = -1;
            float aspect = canvasSize.x / canvasSize.y;
            for (int i = 0; i < canvasTemplates.Length; i++)
            {
                if (aspect == canvasTemplates[i].Aspect)
                {
                    index = i;
                    break;
                }
                else if (aspect < canvasTemplates[i].Aspect)
                {
                    if (i == 0)
                    {
                        index = i;
                    }
                    else
                    {
                        if (aspect - canvasTemplates[i - 1].Aspect < canvasTemplates[i].Aspect - aspect)
                            index = i - 1;
                        else
                            index = i;
                    }
                    break;
                }
            }
            //
            return index;
        }
        public static void Sort(CanvasTemplate[] canvasTemplates)
        {
            for (int i = 0; i < canvasTemplates.Length - 1; i++)
                for (int j = 0; j < canvasTemplates.Length - 1; j++)
                {
                    if (canvasTemplates[j].Aspect > canvasTemplates[j + 1].Aspect)
                    {
                        CanvasTemplate tmp = canvasTemplates[j];
                        canvasTemplates[j] = canvasTemplates[j + 1];
                        canvasTemplates[j + 1] = tmp;
                    }
                }
        }
        #endregion
    }
}
