using KTool;
using KTool.Loading;
using System.Collections;
using UnityEngine;
namespace KTool_Demo.Loading
{
    public class DemoLoader : MonoBehaviour, ILoader
    {
        #region Properties
        [SerializeField]
        private string LoadName;
        [SerializeField]
        [Min(1)]
        private float timeLoad = 1;
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

        #region Loader

        public TrackEntry LoadBegin()
        {
            TrackEntrySource trackLoaderSource = new TrackEntrySource("Loading... " + LoadName);
            StartCoroutine(IE_Delay(trackLoaderSource, timeLoad));
            return trackLoaderSource;
        }
        public void LoadEnd()
        {
            Debug.Log("Load ended: " + LoadName);
        }
        private IEnumerator IE_Delay(TrackEntrySource trackLoaderSource, float delay = 1)
        {
            float time = 0;
            while (time < delay)
            {
                trackLoaderSource.Progress = time / delay;
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            trackLoaderSource.CompleteSuccess();
        }
        #endregion
    }
}
