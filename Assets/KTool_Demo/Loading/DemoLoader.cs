using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Loading;
using KTool;
namespace KTool_Demo.Loading
{
    public class DemoLoader : MonoBehaviour, ILoader
    {
        #region Properties
        [SerializeField]
        private string LoadName;
        [SerializeField]
        [Min(1)]
        private float timeLoad = 1,
            timeInit = 1;
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

        public void GameLoad(Entri entri)
        {
            entri.Name = "Game load: " + LoadName;
            StartCoroutine(IE_Delay(entri, timeLoad));
        }

        public void GameInit(Entri entri)
        {
            entri.Name = "Game Init: " + LoadName;
            StartCoroutine(IE_Delay(entri, timeInit));
        }
        public void GameStart()
        {
            Debug.Log("Game Start: " + LoadName);
        }
        private IEnumerator IE_Delay(Entri entri, float delay = 1)
        {
            float time = 0;
            while (time < delay)
            {
                entri.Progress = time / delay;
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            entri.Done();
        }
        #endregion
    }
}
