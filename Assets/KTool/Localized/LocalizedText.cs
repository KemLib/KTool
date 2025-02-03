using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.Localized
{
    public class LocalizedText : MonoBehaviour
    {
        #region Properties
        public static LocalizedText Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private List<TextControl> texts;

        #endregion Properties

        #region Unity Event
        void Awake()
        {
            if (Instance == null)
            {
                Init();
                return;
            }
            //
            if (Instance.GetInstanceID() == GetInstanceID())
                return;
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if (Instance != null && Instance.GetInstanceID() == GetInstanceID())
            {
                Instance = null;
                if (LocalizedManager.Instance != null)
                    LocalizedManager.Instance.Event_Unregister(LocalizedManager_OnInit, LocalizedManager_OnChangeLanguage);
            }
        }
        #endregion Unity Event

        #region Method
        private void Init()
        {
            Instance = this;
            //
            StartCoroutine(IE_EventRegister());
        }
        private IEnumerator IE_EventRegister()
        {
            while (LocalizedManager.Instance == null)
                yield return new WaitForEndOfFrame();
            LocalizedManager.Instance.Event_Register(LocalizedManager_OnInit, LocalizedManager_OnChangeLanguage);
        }
        private void LocalizedManager_OnInit()
        {
            foreach (TextControl text in texts)
            {
                if (text == null)
                    continue;
                text.OnInit();
            }
        }
        private void LocalizedManager_OnChangeLanguage()
        {
            foreach (TextControl text in texts)
            {
                if (text == null)
                    continue;
                text.OnChangeLanguage();
            }
        }
        #endregion Method

        #region Text
        public int Text_Count()
        {
            return texts.Count;
        }
        public void Text_Add(TextControl text)
        {
            if (text == null)
                return;
            //
            if (!Text_Contains(text))
                return;
            texts.Add(text);
            //
            if (LocalizedManager.Instance == null)
                return;
            text.OnInit();
            if (LocalizedManager.Instance.CurrentLanguage != LocalizedManager.Instance.DefaultLanguage)
                text.OnChangeLanguage();
        }
        public void Text_Remove(TextControl text)
        {
            if (text == null)
                return;
            //
            int index;
            if (Text_Contains(text, out index))
                texts.RemoveAt(index);
        }
        private bool Text_Contains(TextControl text, out int index)
        {
            int id = text.GetInstanceID();
            index = 0;
            foreach (TextControl item in texts)
            {
                if (item.GetInstanceID() == id)
                    return true;
                else
                    index++;
            }
            index = -1;
            return false;
        }
        private bool Text_Contains(TextControl text)
        {
            int id = text.GetInstanceID();
            for (int i = 0; i < texts.Count; i++)
                if (id == texts[i].GetInstanceID())
                    return true;
            return false;
        }
        #endregion
    }
}
