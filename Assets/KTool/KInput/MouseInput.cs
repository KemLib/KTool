using UnityEngine;

namespace KTool.KInput
{
    public class MouseInput : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private LayerMask layerIgnore;
        [SerializeField]
        private float zoomScaleStandalone = 1,
            zoomScaleMobie = 1;

        private bool isInit;
        private Mouse mouse;

        public Mouse Mouse => mouse;
        #endregion Properties

        #region UnityEvent
        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            if (!isInit)
                return;
            mouse.Update();
        }
        #endregion UnityEvent

        #region Method
        public void Init()
        {
            isInit = true;
            mouse = CreateMouseNative();
        }
        #endregion Method

        #region Static Method
        public Mouse CreateMouseNative()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return CreateMouseStandalone();
#elif UNITY_IOS || UNITY_ANDROID
			return CreateMouseMobie();
#endif
        }
        private Mouse CreateMouseStandalone()
        {
            return new MouseStandalone(layerIgnore, zoomScaleStandalone);
        }
        private Mouse CreateMouseMobie()
        {
            return new MouseMobie(layerIgnore, zoomScaleMobie);
        }
        #endregion Static Method
    }
}
