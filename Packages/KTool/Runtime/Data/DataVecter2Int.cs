using UnityEngine;

namespace KTool.Data
{
    public class DataVecter2Int : DataObject
    {
        #region Properties
        private string keyX,
            keyY;
        private int x,
            y;

        public int X
        {
            get => x;
            set
            {
                x = value;
                PlayerPrefs.SetInt(keyX, x);
            }
        }
        public int Y
        {
            get => y;
            set
            {
                y = value;
                PlayerPrefs.SetInt(keyY, y);
            }
        }
        public Vector2Int Value
        {
            get => new Vector2Int(x, y);
            set
            {
                X = value.x;
                Y = value.y;
            }
        }
        #endregion

        #region Construction
        public DataVecter2Int(string keyRoot) : base(keyRoot)
        {
            keyX = Key_Get(DataVecter2.KEY_X);
            keyY = Key_Get(DataVecter2.KEY_Y);
            x = PlayerPrefs.GetInt(keyX, 0);
            y = PlayerPrefs.GetInt(keyY, 0);
        }
        public DataVecter2Int(string keyRoot, Vector2Int defaultValue) : base(keyRoot)
        {
            keyX = Key_Get(DataVecter2.KEY_X);
            keyY = Key_Get(DataVecter2.KEY_Y);
            x = PlayerPrefs.GetInt(keyX, defaultValue.x);
            y = PlayerPrefs.GetInt(keyY, defaultValue.y);
        }
        #endregion

        #region Method

        #endregion
    }
}
