using System.Collections;
using UnityEngine;

namespace KTool.MultiThread
{
    public class CoroutineManager : MonoBehaviour
    {
        #region Properties
        private const string GAME_OBJECT_NAME = "Coroutine_Manager";

        private static CoroutineManager instance;
        public static CoroutineManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject newObject = new GameObject(GAME_OBJECT_NAME);
                    DontDestroyOnLoad(newObject);
                    instance = newObject.AddComponent<CoroutineManager>();
                    instance.Init();
                }
                return instance;
            }
        }
        #endregion

        #region Unity Event

        #endregion

        #region Method
        public void Init()
        {

        }

        #endregion Method

        #region Coroutine
        public Coroutine Coroutine_Start(IEnumerator enumerator)
        {
            return StartCoroutine(enumerator);
        }
        public void Coroutine_Stop(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }
        public void Coroutine_StopAll()
        {
            StopAllCoroutines();
        }
        #endregion
    }
}
