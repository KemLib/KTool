using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Init;
using KTool.Ad;

namespace KPlugin.MaxMediation
{
    public abstract class MaxAd : MonoBehaviour, IInit, IAd
    {
        #region Properties
        public const string ERROR_PUSH_EVENT_FORMAT = "{0} push event {1} fail: {2}";
        private const string EVENT_NAME_ON_AD_CREATED = "OnAdCreated",
            EVENT_NAME_ON_AD_LOADED = "OnAdLoaded",
            EVENT_NAME_ON_AD_DISPLAYED = "OnAdDisplayed",
            EVENT_NAME_ON_AD_CLICKED = "OnAdClicked",
            EVENT_NAME_ON_AD_HIDDEN = "OnAdHidden",
            EVENT_NAME_ON_AD_DESTROY = "OnAdDestroy",
            EVENT_NAME_ON_AD_REVENUEPAID = "OnAdRevenuePaid";
        public const string EVENT_NAME_ON_CLICK = "onClick",
            EVENT_NAME_ON_SHOW_COMPLETE = "onShowComplete",
            EVENT_NAME_ON_REWARD = "onReward",
            EVENT_NAME_ON_AD_REWARD = "onReward",
            EVENT_NAME_ON_AD_EXPANDED = "onAdExpanded";

        [SerializeField]
        private InitType initType;

        private bool isInitBegin,
            isInitEnd;
        private bool initComplete;

        public event IAd.OnAdCreated OnAdCreatedEvent;
        public event IAd.OnAdLoaded OnAdLoadedEvent;
        public event IAd.OnAdDisplayed OnAdDisplayedEvent;
        public event IAd.OnAdClicked OnAdClickedEvent;
        public event IAd.OnAdHidden OnAdHiddenEvent;
        public event IAd.OnAdDestroy OnAdDestroyEvent;
        public event IAd.OnAdRevenuePaid OnAdRevenuePaidEvent;

        public InitType InitType => initType;
        public bool InitComplete
        {
            get => initComplete;
            protected set => initComplete = value;
        }
        protected bool IsInitEnded => isInitEnd;
        public string Name => gameObject.name;
        public abstract AdType AdType
        {
            get;
        }
        public abstract bool IsCreated
        {
            get;
        }
        public abstract bool IsLoaded
        {
            get;
        }
        public abstract bool IsLoading
        {
            get;
        }
        public abstract bool IsShow
        {
            get;
        }
        public abstract bool IsAutoReload
        {
            get;
            set;
        }
        #endregion

        #region Unity Event

        #endregion

        #region Init
        public void InitBegin()
        {
            if (isInitBegin)
                return;
            isInitBegin = true;
            //
            OnInitBegin();
        }
        public void InitEnd()
        {
            if (isInitEnd)
                return;
            isInitEnd = true;
            //
            OnInitEnd();
        }
        protected abstract void OnInitBegin();
        protected abstract void OnInitEnd();
        #endregion

        #region Ad
        public abstract void Create();
        public abstract void Destroy();
        public abstract void Load();

        protected void PushEvent_OnAdCreated(MaxAdType adType, bool isSuccess)
        {
            try
            {
                OnAdCreatedEvent?.Invoke(isSuccess);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, adType.ToString(), EVENT_NAME_ON_AD_CREATED, ex.Message);
                Debug.LogError(message);
            }
        }
        protected void PushEvent_OnAdLoaded(MaxAdType adType, bool isSuccess)
        {
            try
            {
                OnAdLoadedEvent?.Invoke(isSuccess);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, adType.ToString(), EVENT_NAME_ON_AD_LOADED, ex.Message);
                Debug.LogError(message);
            }
        }
        protected void PushEvent_OnAdDisplayed(MaxAdType adType, bool isSuccess)
        {
            try
            {
                OnAdDisplayedEvent?.Invoke(isSuccess);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, adType.ToString(), EVENT_NAME_ON_AD_DISPLAYED, ex.Message);
                Debug.LogError(message);
            }
        }
        protected void PushEvent_OnAdClicked(MaxAdType adType)
        {
            try
            {
                OnAdClickedEvent?.Invoke();
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, adType.ToString(), EVENT_NAME_ON_AD_CLICKED, ex.Message);
                Debug.LogError(message);
            }
        }
        protected void PushEvent_OnAdHidden(MaxAdType adType)
        {
            try
            {
                OnAdHiddenEvent?.Invoke();
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, adType.ToString(), EVENT_NAME_ON_AD_HIDDEN, ex.Message);
                Debug.LogError(message);
            }
        }
        protected void PushEvent_OnAdDestroy(MaxAdType adType)
        {
            try
            {
                OnAdDestroyEvent?.Invoke();
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, adType.ToString(), EVENT_NAME_ON_AD_DESTROY, ex.Message);
                Debug.LogError(message);
            }
        }
        protected void PushEvent_OnAdRevenuePaid(MaxAdType adType, AdRevenuePaid adRevenuePaid)
        {
            try
            {
                OnAdRevenuePaidEvent?.Invoke(adRevenuePaid);
            }
            catch (Exception ex)
            {
                string message = string.Format(ERROR_PUSH_EVENT_FORMAT, adType.ToString(), EVENT_NAME_ON_AD_REVENUEPAID, ex.Message);
                Debug.LogError(message);
            }
        }
        #endregion
    }
}
