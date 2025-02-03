using UnityEngine;

namespace KTool.Data
{
    public class DataVecter3 : DataObject
    {
        #region Properties
        public const string KEY_X = DataVecter2.KEY_X,
            KEY_Y = DataVecter2.KEY_X,
            KEY_Z = "z";

        private string keyX,
            keyY,
            keyZ;
        private float x,
            y,
            z;

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
        public float Z
        {
            get => z;
            set
            {
                z = value;
                PlayerPrefs.SetFloat(keyZ, z);
            }
        }
        public Vector3 Value
        {
            get => new Vector3(x, y, z);
            set
            {
                X = value.x;
                Y = value.y;
                Z = value.z;
            }
        }
        #endregion

        #region Construction
        public DataVecter3(string keyRoot) : base(keyRoot)
        {
            keyX = Key_Get(KEY_X);
            keyY = Key_Get(KEY_Y);
            keyZ = Key_Get(KEY_Z);
            x = PlayerPrefs.GetFloat(keyX, 0);
            y = PlayerPrefs.GetFloat(keyY, 0);
            z = PlayerPrefs.GetFloat(keyZ, 0);
        }
        public DataVecter3(string keyRoot, Vector3 defaultValue) : base(keyRoot)
        {
            keyX = Key_Get(KEY_X);
            keyY = Key_Get(KEY_Y);
            keyZ = Key_Get(KEY_Z);
            x = PlayerPrefs.GetFloat(keyX, defaultValue.x);
            y = PlayerPrefs.GetFloat(keyY, defaultValue.y);
            z = PlayerPrefs.GetFloat(keyZ, defaultValue.z);
        }
        #endregion

        #region Method

        #endregion
    }
}
