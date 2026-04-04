using KTool.Cron;
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
            Debug.Log("Init begin: " + name);
            InitTrackingSource initTrackingSource = new InitTrackingSource(initIndispensable);
            CronObject.Create()
                .Add(ConditionTime.Create(timeInit))
                .Add(CallbackAction<InitTrackingSource>.Create(Init_OnComplete, initTrackingSource))
                .Run();
            return initTrackingSource;
        }

        public void InitEnd()
        {
            Debug.Log("Init end: " + name);
        }

        private void Init_OnComplete(InitTrackingSource initTrackingSource)
        {
            Debug.Log("Init Complete: " + name);
            initTrackingSource.CompleteSuccess();
        }
        #endregion
    }
}
