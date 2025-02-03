using System;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Init
{
    [System.Serializable]
    public class GroupItem
    {
        #region Properties
        private const string ERROR_ITEM_INIT_BEGIN = "Fail to InitBegin in GameObject '{0}' exception: {1}",
            ERROR_ITEM_INIT_END = "Fail to InitBegin in GameObject '{0}' exception: {1}";

        [SerializeField]
        private GameObject[] gameobjects;

        private List<IInit> items;

        public int Count => items.Count;
        public IInit this[int index] => items[index];
        #endregion

        #region Method
        public void Init()
        {
            items = new List<IInit>();
            List<IInit> tmp = new List<IInit>();
            foreach (GameObject gameobject in gameobjects)
            {
                tmp.Clear();
                gameobject.GetComponents<IInit>(tmp);
                if (tmp.Count > 0)
                    items.AddRange(tmp);
            }
        }
        #endregion

        #region Item
        public void Item_Init()
        {
            foreach (IInit item in items)
            {
                try
                {
                    item.InitBegin();
                }
                catch (Exception ex)
                {
                    string message = string.Format(ERROR_ITEM_INIT_BEGIN, item.Name, ex.Message);
                    Debug.LogError(message);
                }
            }
        }
        public void Item_InitEnded()
        {
            foreach (IInit item in items)
            {
                try
                {
                    item.InitEnd();
                }
                catch (Exception ex)
                {
                    string message = string.Format(ERROR_ITEM_INIT_END, item.Name, ex.Message);
                    Debug.LogError(message);
                }
            }
        }
        public float Item_GetProgress()
        {
            int total = items.Count;
            if (total == 0)
                return 1;
            //
            int current = 0;
            foreach (IInit item in items)
                if (item.InitComplete)
                    current++;
            return (float)current / total;
        }
        public bool Item_IsCompleteAll()
        {
            foreach (IInit item in items)
                if (!item.InitComplete)
                    return false;
            return true;
        }
        public bool Item_IsCompleteAllCompulsory()
        {
            foreach (IInit item in items)
                if (item.InitType == InitType.Compulsory && !item.InitComplete)
                    return false;
            return true;
        }
        #endregion
    }
}
