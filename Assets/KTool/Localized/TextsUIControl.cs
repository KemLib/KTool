using UnityEngine;

namespace KTool.Localized
{
    public class TextsUIControl : TextControl
    {
        #region Properties
        [SerializeField]
        private TextItem[] items;
        #endregion Properties

        #region Unity Event
        private void Awake()
        {

        }
        #endregion Unity Event

        #region Method
        public override void OnInit()
        {
            foreach (TextItem item in items)
                item.Init();
        }
        public override void OnChangeLanguage()
        {
            foreach (TextItem item in items)
                item.ChangeLanguage();
        }
        #endregion Method
    }
}
