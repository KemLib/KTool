using KTool.Advertisement.Demo;

namespace KTool.Advertisement
{
    public abstract class AdAppOpen : Ad
    {
        #region Properties
        protected static AdAppOpen instance;
        public static AdAppOpen Instance => instance == null ? AdDemoAppOpen.InstanceAdDemo : instance;

        public override AdType AdType => AdType.AppOpen;
        #endregion

        #region Methods
        public abstract IAdTracking Show();
        #endregion
    }
}
