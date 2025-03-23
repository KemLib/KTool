using UnityEngine;
using UnityEngine.Events;

namespace KTool.Sound
{
    public class SoundManager : MonoBehaviour
    {
        #region Properties
        public static SoundManager Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private SoundBackground soundBG;
        [SerializeField]
        private PoolingSoundItem poolingSoundItem = null;
        [SerializeField]
        private bool isDontDestroy;

        public SoundBackground SoundBG => soundBG;
        #endregion Properties

        #region UnityEvent
        private void Awake()
        {
            if (Instance == null)
            {
                Init();
                if (isDontDestroy)
                    DontDestroyOnLoad(gameObject);
                return;
            }
            if (Instance.GetInstanceID() == GetInstanceID())
                return;
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if (Instance != null && Instance.GetInstanceID() == GetInstanceID())
                Instance = null;
        }
        #endregion UnityEvent

        #region Method
        private void Init()
        {
            Instance = this;
            //
            soundBG.Init();
            poolingSoundItem.Init();
        }
        #endregion

        #region SoundItem
        public bool Sound_GetMute()
        {
            return poolingSoundItem.IsMute;
        }
        public void Sound_SetMute(bool isMute)
        {
            poolingSoundItem.IsMute = isMute;
        }
        public SoundItem Sound_Play(AudioClip clip, float volume = 1, int loop = 1, UnityAction<SoundItem> onComplete = null)
        {
            SoundItem newItem = poolingSoundItem.Item_Create();
            newItem.Play(clip, volume, loop, onComplete);
            return newItem;
        }
        public SoundItem Sound_Play(AudioClip clip, Vector3 position, float volume = 1, int loop = 1, UnityAction<SoundItem> onComplete = null)
        {
            SoundItem newItem = poolingSoundItem.Item_Create();
            newItem.Play(clip, position, volume, loop, onComplete);
            return newItem;
        }
        public SoundItem Sound_Play(AudioClip clip, Transform tagetFollow, float volume = 1, int loop = 1, UnityAction<SoundItem> onComplete = null)
        {
            SoundItem newItem = poolingSoundItem.Item_Create();
            newItem.Play(clip, tagetFollow, volume, loop, onComplete);
            return newItem;
        }
        #endregion
    }
}
