using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Ad;

namespace KPlugin.MaxMediation
{
    public static class MaxUtils
    {
        #region Properties
        public static float DensityScreen => MaxSdkUtils.GetScreenDensity();
        #endregion

        #region Method
        public static Vector2 GetUnityScreen()
        {
            return new Vector2(Screen.width, Screen.height);
        }
        public static Vector2 GetMaxScreen()
        {
            return Convert_UnityScreen_To_MaxScreen(GetUnityScreen());
        }

        public static MaxSdkBase.BannerPosition Convert_BannerPosition(AdPosition positionType)
        {
            switch (positionType)
            {
                case AdPosition.TopLeft:
                    return MaxSdkBase.BannerPosition.TopLeft;
                case AdPosition.TopCenter:
                    return MaxSdkBase.BannerPosition.TopCenter;
                case AdPosition.TopRight:
                    return MaxSdkBase.BannerPosition.TopRight;
                case AdPosition.MidLeft:
                    return MaxSdkBase.BannerPosition.CenterLeft;
                case AdPosition.MidCenter:
                    return MaxSdkBase.BannerPosition.Centered;
                case AdPosition.MidRight:
                    return MaxSdkBase.BannerPosition.CenterRight;
                case AdPosition.BotLeft:
                    return MaxSdkBase.BannerPosition.BottomLeft;
                case AdPosition.BotCenter:
                    return MaxSdkBase.BannerPosition.BottomCenter;
                case AdPosition.BotRight:
                    return MaxSdkBase.BannerPosition.BottomRight;
                default:
                    return MaxSdkBase.BannerPosition.BottomCenter;
            }
        }
        public static MaxSdkBase.AdViewPosition Convert_AdViewPosition(AdPosition positionType)
        {
            switch (positionType)
            {
                case AdPosition.TopLeft:
                    return MaxSdkBase.AdViewPosition.TopLeft;
                case AdPosition.TopCenter:
                    return MaxSdkBase.AdViewPosition.TopCenter;
                case AdPosition.TopRight:
                    return MaxSdkBase.AdViewPosition.TopRight;
                case AdPosition.MidLeft:
                    return MaxSdkBase.AdViewPosition.CenterLeft;
                case AdPosition.MidCenter:
                    return MaxSdkBase.AdViewPosition.Centered;
                case AdPosition.MidRight:
                    return MaxSdkBase.AdViewPosition.CenterRight;
                case AdPosition.BotLeft:
                    return MaxSdkBase.AdViewPosition.BottomLeft;
                case AdPosition.BotCenter:
                    return MaxSdkBase.AdViewPosition.BottomCenter;
                case AdPosition.BotRight:
                    return MaxSdkBase.AdViewPosition.BottomRight;
                default:
                    return MaxSdkBase.AdViewPosition.BottomCenter;
            }
        }
        public static float Convert_UnityScreen_To_MaxScreen(float point)
        {
            return point / DensityScreen;
        }
        public static Vector2 Convert_UnityScreen_To_MaxScreen(Vector2 value)
        {
            float x = Convert_UnityScreen_To_MaxScreen(value.x),
                y = Convert_UnityScreen_To_MaxScreen(value.y);
            return new Vector2(x, y);
        }
        public static float Convert_MaxScreen_To_UnityScreen(float point)
        {
            return point * DensityScreen;
        }
        public static Vector2 Convert_MaxScreen_To_UnityScreen(Vector2 value)
        {
            float x = Convert_MaxScreen_To_UnityScreen(value.x),
                y = Convert_MaxScreen_To_UnityScreen(value.y);
            return new Vector2(x, y);
        }
        #endregion

        #region Banner
        public static Vector2 GetUnityScreen_BannerSize()
        {
            return Convert_MaxScreen_To_UnityScreen(GetMaxScreen_BannerSize());
        }
        public static Vector2 GetMaxScreen_BannerSize()
        {
            if (MaxSdkUtils.IsTablet())
                return new Vector2(728, 90);
            else
                return new Vector2(320, 50);
        }
        public static Vector2 GetUnityScreen_BannerPosition(AdPosition positionType)
        {
            return Convert_MaxScreen_To_UnityScreen(GetMaxScreen_BannerPosition(positionType));
        }
        public static Vector2 GetMaxScreen_BannerPosition(AdPosition positionType)
        {
            Vector2 screenSize = GetMaxScreen(),
                bannerSize = GetMaxScreen_BannerSize();
            switch (positionType)
            {
                case AdPosition.TopLeft:
                    return new Vector2(0, screenSize.y - bannerSize.y);
                case AdPosition.TopCenter:
                    return new Vector2((screenSize.x - bannerSize.x) / 2, screenSize.y - bannerSize.y);
                case AdPosition.TopRight:
                    return new Vector2(screenSize.x - bannerSize.x, screenSize.y - bannerSize.y);
                case AdPosition.MidLeft:
                    return new Vector2(0, (screenSize.y - bannerSize.y) / 2);
                case AdPosition.MidCenter:
                    return new Vector2((screenSize.x - bannerSize.x) / 2, (screenSize.y - bannerSize.y) / 2);
                case AdPosition.MidRight:
                    return new Vector2(screenSize.x - bannerSize.x, (screenSize.y - bannerSize.y) / 2);
                case AdPosition.BotLeft:
                    return new Vector2(0, 0);
                case AdPosition.BotCenter:
                    return new Vector2((screenSize.x - bannerSize.x) / 2, 0);
                case AdPosition.BotRight:
                    return new Vector2(screenSize.x - bannerSize.x, 0);
                default:
                    return Vector2.zero;
            }
        }
        #endregion

        #region MRec
        public static Vector2 GetUnityScreen_MRecSize()
        {
            return Convert_MaxScreen_To_UnityScreen(GetMaxScreen_MRecSize());
        }
        public static Vector2 GetMaxScreen_MRecSize()
        {
            return new Vector2(300, 250);
        }
        public static Vector2 GetUnityScreen_MRecPosition(AdPosition positionType)
        {
            return Convert_MaxScreen_To_UnityScreen(GetMaxScreen_MRecPosition(positionType));
        }
        public static Vector2 GetMaxScreen_MRecPosition(AdPosition positionType)
        {
            Vector2 screenSize = GetMaxScreen(),
                mrecSize = GetMaxScreen_MRecSize();
            switch (positionType)
            {
                case AdPosition.TopLeft:
                    return new Vector2(0, screenSize.y - mrecSize.y);
                case AdPosition.TopCenter:
                    return new Vector2((screenSize.x - mrecSize.x) / 2, screenSize.y - mrecSize.y);
                case AdPosition.TopRight:
                    return new Vector2(screenSize.x - mrecSize.x, screenSize.y - mrecSize.y);
                case AdPosition.MidLeft:
                    return new Vector2(0, (screenSize.y - mrecSize.y) / 2);
                case AdPosition.MidCenter:
                    return new Vector2((screenSize.x - mrecSize.x) / 2, (screenSize.y - mrecSize.y) / 2);
                case AdPosition.MidRight:
                    return new Vector2(screenSize.x - mrecSize.x, (screenSize.y - mrecSize.y) / 2);
                case AdPosition.BotLeft:
                    return new Vector2(0, 0);
                case AdPosition.BotCenter:
                    return new Vector2((screenSize.x - mrecSize.x) / 2, 0);
                case AdPosition.BotRight:
                    return new Vector2(screenSize.x - mrecSize.x, 0);
                default:
                    return Vector2.zero;
            }
        }
        #endregion
    }
}
