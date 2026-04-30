using System.Collections.Generic;
using UnityEngine;

namespace KTool.Cron
{
    internal class CronManager : MonoBehaviour
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

        private List<CronObject> crons;
        #endregion

        #region Unity Event
        private void Update()
        {
            Cron_Update();
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
        }
        #endregion Method

        #region Cron
        private void Cron_Update()
        {
            int index = 0;
            while (index < crons.Count)
            {
                CronObject cron = crons[index];
                cron.Update();
                if (cron.IsComplete)
                    crons.RemoveAt(index);
                else
                    index++;
            }
        }
        internal void Cron_Add(CronObject cron)
        {
            if (cron == null || cron.IsComplete)
                return;
            cron.Update();
            if (cron.IsComplete)
                return;
            crons.Add(cron);
        }
        #endregion
    }
}
