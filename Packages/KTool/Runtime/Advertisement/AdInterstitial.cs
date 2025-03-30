﻿using KTool.Advertisement.Demo;

namespace KTool.Advertisement
{
    public abstract class AdInterstitial : Ad
    {
        #region Properties
        protected static AdInterstitial instance;
        public static AdInterstitial Instance
        {
            get
            {
                if (instance == null)
                    return AdInterstitialDemo.InstanceAdInterstitial;
                else
                    return instance;
            }
        }

        public override AdType AdType => AdType.Interstitial;
        #endregion

        #region Methods
        public abstract AdInterstitialTracking Show();
        #endregion
    }
}
