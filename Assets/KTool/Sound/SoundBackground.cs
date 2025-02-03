using UnityEngine;

namespace KTool.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundBackground : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        private AudioSource audioSource;

        public AudioClip Clip
        {
            get => audioSource.clip;
            private set => audioSource.clip = value;
        }
        public bool IsMute
        {
            get => audioSource.mute;
            set => audioSource.mute = value;
        }
        public bool IsLoop
        {
            get => audioSource.loop;
            set => audioSource.loop = value;
        }
        public bool IsPlaying => audioSource.isPlaying;
        public float Volume
        {
            get => audioSource.volume;
            set => audioSource.volume = value;
        }
        public int Priority
        {
            get => audioSource.priority;
            set => audioSource.priority = value;
        }
        public float MinDistance
        {
            get => audioSource.minDistance;
            set => audioSource.minDistance = value;
        }
        public float MaxDistance
        {
            get => audioSource.maxDistance;
            set => audioSource.maxDistance = value;
        }
        #endregion

        #region Unity Event
        #endregion

        #region Method
        public void Init()
        {

        }
        #endregion

        #region Music
        public void Bg_Play(AudioClip clip)
        {
            Clip = clip;
            audioSource.Play();
        }
        public void Music_Stop()
        {
            if (!IsPlaying)
                return;
            audioSource.Stop();
        }
        public void Music_Pause()
        {
            if (!IsPlaying)
                return;
            audioSource.Pause();
        }
        public void Music_UnPause()
        {
            if (IsPlaying)
                return;
            audioSource.UnPause();
        }
        #endregion Music
    }
}
