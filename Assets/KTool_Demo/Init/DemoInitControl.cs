using KTool.Init;
using System.Collections;
using UnityEngine;

namespace KTool_Demo.Init
{
    public class DemoInitControl : MonoBehaviour, IIniter
    {
        #region Properties
        [SerializeField]
        private bool requiredConditions;
        [SerializeField]
        private float timeInit;

        public bool RequiredConditions => requiredConditions;
        #endregion

        #region Unity Event
        #endregion

        #region Method
        public InitTracking InitBegin()
        {
            Debug.Log("Demo Init Begin");
            InitTrackingSource initTrackingSource = new InitTrackingSource();
            StartCoroutine(IE_Init(initTrackingSource));
            return initTrackingSource;
        }

        public void InitEnd()
        {
            Debug.Log("Demo Init Ended");
        }

        private IEnumerator IE_Init(InitTrackingSource initTrackingSource)
        {
            float time = 0;
            while (time < timeInit)
            {
                time += Time.deltaTime;
                initTrackingSource.Progress = time / timeInit;
                yield return new WaitForEndOfFrame();
            }
            initTrackingSource.CompleteSuccess();
            Debug.Log("Init Complete: " + gameObject.name);
        }
        #endregion
    }
}
