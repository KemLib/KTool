using System.Collections.Generic;
using UnityEngine;

namespace KTool.Cron
{
    public class CronManager : MonoBehaviour
    {
        #region Properties
        private const string GAME_OBJECT_NAME = "KTool_CronUpdateManager";

        private static CronManager instance;
        internal static CronManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject newObject = new GameObject(GAME_OBJECT_NAME);
                    DontDestroyOnLoad(newObject);
                    //
                    instance = newObject.AddComponent<CronManager>();
                    instance.Init();
                }
                return instance;
            }
        }

        private List<CronObject> crons,
            cronTmp;
        #endregion

        #region Unity Event
        private void Update()
        {
            Cron_Update();
        }
        private void LateUpdate()
        {
            Cron_LateUpdate();
        }
        private void OnDestroy()
        {
            if (instance != null && instance.GetInstanceID() == GetInstanceID())
                instance = null;
        }
        #endregion

        #region Method
        private void Init()
        {
            crons = new List<CronObject>();
            cronTmp = new List<CronObject>();
        }
        #endregion Method

        #region Cron
        private void Cron_Update()
        {
            CronObject cron;
            int index = 0;
            while (index < crons.Count)
            {
                cron = crons[index];
                cron.Update();
                if (cron.IsComplete)
                    crons.RemoveAt(index);
                else
                    index++;
            }
        }
        private void Cron_LateUpdate()
        {
            crons.AddRange(cronTmp);
            cronTmp.Clear();
        }
        internal void Cron_Add(CronObject cron)
        {
            if (cron == null || cron.IsComplete)
                return;
            cronTmp.Add(cron);
        }
        #endregion
    }
}
