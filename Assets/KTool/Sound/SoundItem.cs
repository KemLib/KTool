using UnityEngine;
using UnityEngine.Events;

namespace KTool.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundItem : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        private PoolingSoundItem pooling;
        private Transform tagetFollow;
        private UnityAction<SoundItem> onComplete;
        private int instanceID;
        private bool isPlay,
            isComplete;
        private int maxNumberPlay,
            currentNumberPlay;

        private AudioSource AudioSource => audioSource;
        public Transform TagetFollow => tagetFollow;
        public AudioClip Clip
        {
            get => audioSource.clip;
            private set => audioSource.clip = value;
        }
        public int InstanceID => instanceID;
        public bool IsLoop
        {
            get => AudioSource.loop;
            set => AudioSource.loop = value;
        }
        public bool IsMute
        {
            set => AudioSource.mute = value;
            get => AudioSource.mute;
        }
        public bool IsPlaying => AudioSource.isPlaying;
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
        public bool IsPlay => isPlay;
        public bool IsComplete => isComplete;
        public int MaxNumberPlay => maxNumberPlay;
        public int CurrentNumberPlay => currentNumberPlay;
        #region Properties

        #endregion

        #region Unity Event
        // Update is called once per frame
        private void LateUpdate()
        {
            if (IsComplete)
                return;
            Update_Follow();
            if (IsPlay)
                Update_Sound();
        }
        private void Update_Follow()
        {
            if (tagetFollow == null)
                return;
            transform.position = tagetFollow.position;
        }
        private void Update_Sound()
        {
            if (IsPlaying)
                return;
            currentNumberPlay++;
            //
            if (IsLoop || currentNumberPlay < maxNumberPlay)
                AudioSource.Play();
            else
                OnComplete();
        }
        #endregion

        #region Method
        public void Init(PoolingSoundItem pooling)
        {
            this.pooling = pooling;
            instanceID = GetInstanceID();
        }
        private void OnComplete()
        {
            isPlay = false;
            isComplete = true;
            onComplete?.Invoke(this);
            AudioSource.clip = null;
            onComplete = null;
            pooling.Item_Destroy(this);
        }
        #endregion

        #region Sound
        public void Play(AudioClip clip, float volume = 1, int loop = 1, UnityAction<SoundItem> onComplete = null)
        {
            Clip = clip;
            this.onComplete = onComplete;
            transform.localPosition = Vector3.zero;
            //
            Play(volume, loop);
        }
        public void Play(AudioClip clip, Vector3 position, float volume = 1, int loop = 1, UnityAction<SoundItem> onComplete = null)
        {
            Clip = clip;
            this.onComplete = onComplete;
            transform.position = position;
            //
            Play(volume, loop);
        }
        public void Play(AudioClip clip, Transform tagetFollow, float volume = 1, int loop = 1, UnityAction<SoundItem> onComplete = null)
        {
            if (tagetFollow == null)
            {
                Play(clip, volume, loop, onComplete);
                return;
            }
            Play(clip, tagetFollow.transform, volume, loop, onComplete);
            this.tagetFollow = tagetFollow;
        }
        private void Play(float volume, int loop)
        {
            if (Clip == null)
            {
                OnComplete();
                return;
            }
            //
            Volume = volume;
            IsLoop = (loop <= 0);
            maxNumberPlay = loop;
            currentNumberPlay = 0;
            isPlay = true;
            isComplete = false;
            AudioSource.Play();
        }
        public void Pause()
        {
            if (IsComplete)
                return;
            //
            isPlay = false;
            AudioSource.Pause();
        }
        public void UnPause()
        {
            if (IsComplete)
                return;
            //
            isPlay = true;
            AudioSource.UnPause();
        }
        public void Stop()
        {
            if (IsComplete)
                return;
            //
            AudioSource.Stop();
            OnComplete();
        }
        #endregion
    }
}
