using KTool;
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
        public TrackEntry InitBegin()
        {
            Debug.Log("Demo Init Begin");
            TrackEntrySource trackEntrySource = new TrackEntrySource(gameObject.name);
            StartCoroutine(IE_Init(trackEntrySource));
            return trackEntrySource;
        }

        public void InitEnd()
        {
            Debug.Log("Demo Init Ended");
        }

        private IEnumerator IE_Init(TrackEntrySource trackEntrySource)
        {
            float time = 0;
            while (time < timeInit)
            {
                time += Time.deltaTime;
                trackEntrySource.Progress = time / timeInit;
                yield return new WaitForEndOfFrame();
            }
            trackEntrySource.CompleteSuccess();
            Debug.Log("Init Complete: " + gameObject.name);
        }
        #endregion
    }
}
