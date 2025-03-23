using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KTool.MenuAnim
{
    public class MenuAnimControl : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private bool isShow;
        [SerializeField]
        private UpdateType updateType;
        [SerializeField]
        private bool unscaleTime;
        [SerializeField]
        private Anim animHide,
            animShow;

        private Coroutine coroutine;

        public bool IsShow => isShow;
        public bool IsPlay => (animHide.IsPlay || animShow.IsPlay);
        #endregion Properties

        #region UnityEvent
        private void OnDestroy()
        {
            Stop();
        }
        #endregion UnityEvent

        #region Method
        public void PlayHide(float delay)
        {
            PlayHide(delay, null);
        }
        public void PlayHide(UnityAction onComplete)
        {
            PlayHide(0, onComplete);
        }
        public void PlayHide(float delay = 0, UnityAction onComplete = null)
        {
            if (!isShow)
                return;
            Stop();
            isShow = false;
            coroutine = StartCoroutine(IE_Hide(animHide, delay, onComplete));
        }
        public void PlayShow(float delay)
        {
            PlayShow(delay, null);
        }
        public void PlayShow(UnityAction onComplete)
        {
            PlayShow(0, onComplete);
        }
        public void PlayShow(float delay = 0, UnityAction onComplete = null)
        {
            if (isShow)
                return;
            Stop();
            isShow = true;
            coroutine = StartCoroutine(IE_Show(animShow, delay, onComplete));
        }
        public void Stop()
        {
            if (!IsPlay)
                return;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            if (animHide.IsPlay)
                animHide.Stop();
            if (animShow.IsPlay)
                animShow.Stop();
        }
        public void ManualUpdate()
        {
            if (updateType != UpdateType.Manual)
                return;
            float deltaTime = (unscaleTime ? Time.unscaledDeltaTime : Time.deltaTime),
                unscaleDeltaTime = (unscaleTime ? Time.unscaledDeltaTime : Time.deltaTime);
            if (animHide.IsPlay)
                animHide.ManualUpdate(deltaTime, unscaleDeltaTime);
            else
                animShow.ManualUpdate(deltaTime, unscaleDeltaTime);
        }
        private IEnumerator IE_Hide(Anim anim, float delay = 0, UnityAction onComplete = null)
        {
            if (delay > 0)
            {
                if (unscaleTime)
                    yield return new WaitForSecondsRealtime(delay);
                else
                    yield return new WaitForSeconds(delay);
            }
            //
            anim.Start(updateType, unscaleTime);
            while (anim.IsPlay)
                yield return new WaitForEndOfFrame();
            anim.SetObjectActive(false);
            //
            coroutine = null;
            onComplete?.Invoke();
        }
        private IEnumerator IE_Show(Anim anim, float delay = 0, UnityAction onComplete = null)
        {
            if (delay > 0)
            {
                if (unscaleTime)
                    yield return new WaitForSecondsRealtime(delay);
                else
                    yield return new WaitForSeconds(delay);
            }
            //
            anim.SetObjectActive(true);
            anim.Start(updateType, unscaleTime);
            while (anim.IsPlay)
                yield return new WaitForEndOfFrame();
            //
            coroutine = null;
            onComplete?.Invoke();
        }
        #endregion Method
    }
}
