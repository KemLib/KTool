using UnityEngine;

namespace KTool.Data
{
    public class DataVecter3Int : DataObject
    {
        #region Properties
        private string keyX,
            keyY,
            keyZ;
        private int x,
            y,
            z;

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
        public int Z
        {
            get => z;
            set
            {
                z = value;
                PlayerPrefs.SetInt(keyZ, z);
            }
        }
        public Vector3Int Value
        {
            get => new Vector3Int(x, y, z);
            set
            {
                X = value.x;
                Y = value.y;
                Z = value.z;
            }
        }
        #endregion

        #region Construction
        public DataVecter3Int(string keyRoot) : base(keyRoot)
        {
            keyX = Key_Get(DataVecter3.KEY_X);
            keyY = Key_Get(DataVecter3.KEY_Y);
            keyZ = Key_Get(DataVecter3.KEY_Z);
            x = PlayerPrefs.GetInt(keyX, 0);
            y = PlayerPrefs.GetInt(keyY, 0);
            z = PlayerPrefs.GetInt(keyZ, 0);
        }
        public DataVecter3Int(string keyRoot, Vector3Int defaultValue) : base(keyRoot)
        {
            keyX = Key_Get(DataVecter3.KEY_X);
            keyY = Key_Get(DataVecter3.KEY_Y);
            keyZ = Key_Get(DataVecter3.KEY_Z);
            x = PlayerPrefs.GetInt(keyX, defaultValue.x);
            y = PlayerPrefs.GetInt(keyY, defaultValue.y);
            z = PlayerPrefs.GetInt(keyZ, defaultValue.z);
        }
        #endregion

        #region Method

        #endregion
    }
}
