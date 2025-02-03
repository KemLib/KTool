using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using KTool.Init;
using Firebase;

namespace KPlugin.Firebase
{
    public class FirebaseManager : MonoBehaviour, IInit
    {
        #region Properties
        public static FirebaseManager Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private InitType initType;

        private bool initComplete;
        private bool isAvailable;
        private FirebaseApp fbApp;

        public string Name => gameObject.name;
        public InitType InitType => initType;
        public bool InitComplete => initComplete;
        public bool IsAvailable => isAvailable;
        public FirebaseApp FbApp => fbApp;
        #endregion

        #region Unity Event

        #endregion

        #region Method

        #endregion

        #region Init
        public void InitBegin()
        {
            if (InitComplete)
                return;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //
            Firebase_Init();
        }

        public void InitEnd()
        {

        }
        #endregion

        #region Firebase
        private void Firebase_Init()
        {
            StartCoroutine(Firebase_IE_Init());
        }
        private IEnumerator Firebase_IE_Init()
        {
            Task<DependencyStatus> task = FirebaseApp.CheckAndFixDependenciesAsync();
            while (!task.IsCompleted)
                yield return new WaitForEndOfFrame();

            if (task.Result == DependencyStatus.Available)
            {
                fbApp = FirebaseApp.DefaultInstance;
                isAvailable = true;
            }
            else
            {
                isAvailable = false;
            }
            initComplete = true;
        }
        #endregion
    }
}
