using UnityEngine;

namespace KTool.Ad
{
    public interface IAdBanner : IAd
    {
        #region Properties
        public static IAdBanner Instance => AdManager.Instance.Get<IAdBanner>(0);

        event OnAdExpanded OnAdExpandedEvent;

        Vector2 Position
        {
            get;
        }
        Vector2 Size
        {
            get;
        }
        AdPosition PositionType
        {
            get;
        }
        AdSize SizeType
        {
            get;
        }
        bool IsExpanded
        {
            get;
        }
        bool IsReady
        {
            get;
        }
        #endregion

        #region Method
        void Create(AdPosition positionType, AdSize sizeType);
        void Create(Vector2 position, Vector2 size);
        void Show();
        void Hide();
        #endregion

        #region Instance
        static AdType Instance_AdType => (Instance == null ? AdType.Banner : Instance.AdType);
        static bool Instance_IsCreated => (Instance == null ? false : Instance.IsCreated);
        static bool Instance_IsLoaded => (Instance == null ? false : Instance.IsLoaded);
        static bool Instance_IsLoading => (Instance == null ? false : Instance.IsLoading);
        static bool Instance_IsShow => (Instance == null ? false : Instance.IsShow);
        static bool Instance_IsAutoReload
        {
            get => (Instance == null ? false : Instance.IsAutoReload);
            set
            {
                if (Instance == null)
                    return;
                Instance.IsAutoReload = value;
            }
        }
        static Vector2 Instance_Position => (Instance == null ? Vector2.zero : Instance.Position);
        static Vector2 Instance_Size => (Instance == null ? Vector2.zero : Instance.Size);
        static AdPosition Instance_PositionType => (Instance == null ? AdPosition.Custom : Instance.PositionType);
        static AdSize Instance_SizeType => (Instance == null ? AdSize.Custom : Instance.SizeType);
        static bool Instance_IsExpanded => (Instance == null ? false : Instance.IsExpanded);

        static void Instance_Create()
        {
            if (Instance == null)
                return;
            Instance.Create();
        }
        static void Instance_Create(AdPosition positionType, AdSize sizeType)
        {
            if (Instance == null)
                return;
            Instance.Create(positionType, sizeType);
        }
        static void Instance_Create(Vector2 position, Vector2 size)
        {
            if (Instance == null)
                return;
            Instance.Create(position, size);
        }
        static void Instance_Load()
        {
            if (Instance == null)
                return;
            Instance.Load();
        }
        static void Instance_Destroy()
        {
            if (Instance == null)
                return;
            Instance.Destroy();
        }
        static void Instance_Show()
        {
            if (Instance == null)
                return;
            Instance.Show();
        }
        static void Instance_Hide()
        {
            if (Instance == null)
                return;
            Instance.Hide();
        }
        #endregion
    }
}
