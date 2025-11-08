using KTool.Init;
using System.Collections;
using UnityEngine;

namespace KTool_Demo.Init
{
    public class DemoInitControl : MonoBehaviour, IIniter
    {
        #region Properties
        [SerializeField]
        private bool initIndispensable;
        [SerializeField]
        private float timeInit;
        #endregion

        #region Unity Event
        #endregion

        #region Method
        public IInitTracking InitBegin()
        {
            InitTrackingSource initTrackingSource = new InitTrackingSource(initIndispensable);
            StartCoroutine(IE_Init(initTrackingSource));
            return initTrackingSource;
        }

        public void InitEnd()
        {
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
        }
        #endregion
    }
}
