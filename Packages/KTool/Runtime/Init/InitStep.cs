using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Init
{
    [System.Serializable]
    public class InitStep
    {
        #region Properties
        [SerializeField]
        private string stepName;
        [SerializeField]
        private GameObject[] gameobjects;

        private List<IIniter> items;
        private List<IInitTracking> listInitTracking;

        public string StepName => stepName;
        #endregion

        #region Method
        public void Init()
        {
            items = GetAll_Initer(gameobjects);
            listInitTracking = new List<IInitTracking>();
        }
        private static List<IIniter> GetAll_Initer(GameObject[] gameobjects)
        {
            List<IIniter> result = new();
            if(gameobjects == null || gameobjects.Length == 0)
                return result;
            //
            List<IIniter> tmp = new();
            foreach (GameObject gameobject in gameobjects)
            {
                if(gameobject == null)
                    continue;
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
                IInitTracking initTracking;
                try{
                    initTracking = items[index].InitBegin();
                }
                catch(Exception ex)
                {
                    Debug.LogError(ex);
                    initTracking = new InitTrackingSource(true, ex.Message);
                }
                //
                if(initTracking is not null)
                    listInitTracking.Add(initTracking);
                index++;
            }
        }
        public void Item_InitEnded()
        {
            for (int i = 0; i < items.Count; i++)
            {
                try
                {
                    items[i].InitEnd();
                }
                catch(Exception ex)
                {
                    Debug.Log(ex);
                }
            }
            //
            items.Clear();
            listInitTracking.Clear();
        }
        public float Item_GetProgress()
        {
            if (listInitTracking.Count == 0)
                return 1;
            //
            float totalProgress = 0;
            foreach (var value in listInitTracking)
                totalProgress += value.Progress;
            return totalProgress / listInitTracking.Count;
        }
        public bool Item_IsCompleteAll(out bool isSuccessIndispensable)
        {
            isSuccessIndispensable = true;
            foreach (var value in listInitTracking)
            {
                if (value.IsComplete)
                {
                    if(value.Indispensable && !value.IsSuccessfully)
                        isSuccessIndispensable = false;
                }
                else
                {
                    isSuccessIndispensable = false;
                    return false;
                }
            }
            return true;
        }
        public bool Item_IsCompleteIndispensable(out bool isSuccessIndispensable)
        {
            isSuccessIndispensable = true;
            foreach (var value in listInitTracking)
            {
                if (!value.Indispensable)
                    continue;
                if(!value.IsComplete)
                {
                    isSuccessIndispensable = false;
                    return false;
                }
                if(!value.IsSuccessfully)
                    isSuccessIndispensable = false;
            }
            return true;
        }
        #endregion
    }
}
