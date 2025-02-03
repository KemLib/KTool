using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.MenuAnim
{
    [System.Serializable]
    public class Anim
    {
        #region Properties
        [SerializeField]
        private string name;
        [SerializeReference]
        private Item[] items;

        private bool isInit;
        private bool isComplete;
        private UnityAction onComplete;

        public bool IsPlay
        {
            get
            {
                for (int i = 0; i < items.Length; i++)
                    if (items[i].IsPlay)
                        return true;
                return false;
            }
        }
        public bool IsComplete => isComplete;
        #endregion Properties

        #region Method
        private void Init()
        {
            if (isInit)
                return;
            isInit = true;
            //
            for (int i = 0; i < items.Length; i++)
                items[i].Init(this);
        }
        public void SetObjectActive(bool isActive)
        {
            for (int i = 0; i < items.Length; i++)
                items[i].SetObjectActive(isActive);
        }
        public void Start(UpdateType updateType, bool unscaleTime, UnityAction onComplete = null)
        {
            Init();
            if (IsPlay)
                return;
            //
            this.onComplete = onComplete;
            isComplete = false;
            for (int i = 0; i < items.Length; i++)
                items[i].Start(updateType, unscaleTime);
        }
        public void Stop()
        {
            Init();
            if (!IsPlay)
                return;
            //
            isComplete = false;
            for (int i = 0; i < items.Length; i++)
                items[i].Stop();
        }
        public void ManualUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if (!IsPlay)
                return;
            for (int i = 0; i < items.Length; i++)
                items[i].ManualUpdate(deltaTime, unscaleDeltaTime);
        }
        public void OnComplete(Item item)
        {
            if (IsPlay)
                return;
            isComplete = true;
            onComplete?.Invoke();
        }
        #endregion Method
    }
}
