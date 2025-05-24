using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Init
{
    [System.Serializable]
    public class InitStep
    {
        #region Properties
        private const string ERROR_ITEM_INIT_BEGIN = "Fail to InitBegin in object [{0}] exception: {1}",
            ERROR_ITEM_INIT_END = "Fail to InitEnd in object [{0}] exception: {1}";

        [SerializeField]
        private string stepName;
        [SerializeField]
        private GameObject[] gameobjects;

        private List<IIniter> items;
        private Dictionary<IIniter, TrackEntry> dicTrackEntry;

        public string StepName => stepName;
        #endregion

        #region Method
        public void Init()
        {
            items = GetAll_Initer(gameobjects);
            dicTrackEntry = new Dictionary<IIniter, TrackEntry>();
        }
        private static List<IIniter> GetAll_Initer(GameObject[] gameobjects)
        {
            List<IIniter> result = new List<IIniter>(),
                tmp = new List<IIniter>();
            foreach (GameObject gameobject in gameobjects)
            {
                gameobject.GetComponents<IIniter>(tmp);
                if (tmp.Count > 0)
                    result.AddRange(tmp);
            }
            return result;
        }
        #endregion

        #region Item
        public void Item_Init()
        {
            int index = 0;
            while (index < items.Count)
            {
                TrackEntry trackEntry = items[index].InitBegin();
                if (trackEntry is not null)
                    dicTrackEntry.Add(items[index], trackEntry);
                index++;
            }

        }
        public void Item_InitEnded()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].InitEnd();
            }
            //
            items.Clear();
            dicTrackEntry.Clear();
        }
        public float Item_GetProgress()
        {
            if (dicTrackEntry.Count == 0)
                return 1;
            //
            float totalProgress = 0;
            foreach (TrackEntry trackEntry in dicTrackEntry.Values)
                totalProgress += trackEntry.Progress;
            return totalProgress / dicTrackEntry.Count;
        }
        public bool Item_IsCompleteAll()
        {
            foreach (TrackEntry trackEntry in dicTrackEntry.Values)
                if (!trackEntry.IsComplete)
                    return false;
            return true;
        }
        public bool Item_IsCompleteAllRequired()
        {
            foreach (IIniter initer in dicTrackEntry.Keys)
                if (initer.RequiredConditions && !dicTrackEntry[initer].IsComplete)
                    return false;
            return true;
        }
        #endregion
    }
}
