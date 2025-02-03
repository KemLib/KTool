using KTool.Init;
using System.Collections;
using UnityEngine;

namespace KTool_Demo.Init
{
    public class DemoInitControl : MonoBehaviour, IInit
    {
        #region Properties
        [SerializeField]
        private InitType initType;
        [SerializeField]
        private float timeInit;

        private bool initComplete;

        public string Name => gameObject.name;
        public InitType InitType => initType;
        public bool InitComplete => initComplete;
        #endregion

        #region Unity Event		
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Method
        public void InitBegin()
        {
            StartCoroutine(IE_Init());
        }

        public void InitEnd()
        {
            Debug.Log("Demo Init Ended");
        }

        private IEnumerator IE_Init()
        {
            if (timeInit > 0)
                yield return new WaitForSecondsRealtime(timeInit);
            initComplete = true;
            Debug.Log("Init Complete: " + gameObject.name);
        }
        #endregion
    }
}
