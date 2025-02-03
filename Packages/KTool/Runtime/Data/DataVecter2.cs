using UnityEngine;

namespace KTool.Data
{
    public class DataVecter2 : DataObject
    {
        #region Properties
        public const string KEY_X = "x",
            KEY_Y = "y";

        private string keyX,
            keyY;
        private float x,
            y;

        public float X
        {
            get => x;
            set
            {
                x = value;
                PlayerPrefs.SetFloat(keyX, x);
            }
        }
        public float Y
        {
            get => y;
            set
            {
                y = value;
                PlayerPrefs.SetFloat(keyY, y);
            }
        }
        public Vector2 Value
        {
            get => new Vector2(x, y);
            set
            {
                X = value.x;
                Y = value.y;
            }
        }
        #endregion

        #region Construction
        public DataVecter2(string keyRoot) : base(keyRoot)
        {
            keyX = Key_Get(KEY_X);
            keyY = Key_Get(KEY_Y);
            x = PlayerPrefs.GetFloat(keyX, 0);
            y = PlayerPrefs.GetFloat(keyY, 0);
        }
        public DataVecter2(string keyRoot, Vector2 defaultValue) : base(keyRoot)
        {
            keyX = Key_Get(KEY_X);
            keyY = Key_Get(KEY_Y);
            x = PlayerPrefs.GetFloat(keyX, defaultValue.x);
            y = PlayerPrefs.GetFloat(keyY, defaultValue.y);
        }
        #endregion

        #region Method

        #endregion
    }
}
