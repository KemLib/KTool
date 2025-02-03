using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Attribute;
using KTool;
using KTool.MultiThread;

namespace Assets.KTool_Demo.MultiThread
{
    public class DemoMultiThread : MonoBehaviour
    {
        #region Properties

        #endregion

        #region Unity Event		
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Method
        public void Init()
        {
            ThreadManager.Instance.Add(new KTaskCountdown(1, null, ((Result result) =>
            {
                Debug.Log("Done 1");
            })));
            ThreadManager.Instance.Add(new KTaskCountdown(2, null, ((Result result) =>
            {
                Debug.Log("Done 2");
            })));
            ThreadManager.Instance.Add(new KTaskCountdown(3, null, ((Result result) =>
            {
                Debug.Log("Done 3");
            })));
        }
        #endregion
    }
}
