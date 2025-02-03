using System.Collections.Generic;
using UnityEngine;

namespace KTool.Sound
{
    public class PoolingSoundItem : MonoBehaviour
    {
        #region Properties

        [SerializeField]
        private SoundItem prefabItem;
        [SerializeField]
        private Transform tfItem,
            tfTrash;
        [SerializeField]
        [Min(0)]
        private int trashMax = 20;

        private List<SoundItem> items,
            trash;

        public bool IsMute
        {
            set
            {
                prefabItem.IsMute = value;
                foreach (var item in items)
                    item.IsMute = value;
                foreach (var item in trash)
                    item.IsMute = value;
            }
            get
            {
                return prefabItem.IsMute;
            }
        }
        #endregion Properties

        #region Unity Event

        #endregion

        #region Method
        public void Init()
        {
            items = new List<SoundItem>();
            trash = new List<SoundItem>();
        }
        #endregion

        #region Item
        public SoundItem Item_Create()
        {
            SoundItem item;
            if (trash.Count == 0)
            {
                item = Instantiate(prefabItem, Vector3.zero, Quaternion.identity, tfItem);
                item.Init(this);
            }
            else
            {
                item = trash[0];
                trash.RemoveAt(0);
                item.transform.SetParent(tfItem);
            }
            item.gameObject.SetActive(true);
            items.Add(item);
            return item;
        }
        public void Item_Destroy(SoundItem item)
        {
            int index = 0;
            while (index < items.Count)
            {
                if (items[index].InstanceID == item.InstanceID)
                {
                    items.RemoveAt(index);
                    break;
                }
                index++;
            }
            //
            if (trash.Count < trashMax)
            {
                item.gameObject.SetActive(false);
                item.transform.SetParent(tfTrash);
                trash.Add(item);
                return;
            }
            //
            Destroy(item.gameObject);
        }
        #endregion
    }
}
