using KTool.Attribute;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KTool.Init
{
    public class LoadScene : MonoBehaviour, IIniter
    {
        #region Properties
        private const string TASK_NAME_FORMAT = "Load Scene: {0}";
        private const string ERROR_SCENE_NOT_FOUND = "scene {0} not found";
        private const float MAX_LOAD_PROGRESS = 0.9f;

        [SerializeField]
        [SelectScene]
        private string scene;
        [SerializeField]
        private LoadSceneMode mode;
        [SerializeField]
        private bool requiredConditions;

        private AsyncOperation aoLoadScene;

        public bool RequiredConditions => requiredConditions;
        #endregion

        #region Unity Event

        #endregion

        #region Method
        public TrackEntry InitBegin()
        {
            string taskName = string.Format(TASK_NAME_FORMAT, scene);
            try
            {
                aoLoadScene = SceneManager.LoadSceneAsync(scene, mode);
                if (aoLoadScene == null)
                {
                    string errorMessage = string.Format(ERROR_SCENE_NOT_FOUND, scene);
                    return TrackEntrySource.CreateTraskEntryFail(taskName, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return TrackEntrySource.CreateTraskEntryFail(taskName, ex.Message);
            }
            //
            aoLoadScene.allowSceneActivation = false;
            TrackEntrySource trackEntrySource = new TrackEntrySource(taskName);
            StartCoroutine(IE_LoadScene(trackEntrySource));
            return trackEntrySource;
        }

        public void InitEnd()
        {
            aoLoadScene.allowSceneActivation = true;
            aoLoadScene = null;
        }

        private IEnumerator IE_LoadScene(TrackEntrySource trackEntrySource)
        {
            while (aoLoadScene.progress < MAX_LOAD_PROGRESS)
            {
                trackEntrySource.Progress = aoLoadScene.progress;
                yield return new WaitForEndOfFrame();
            }
            trackEntrySource.CompleteSuccess();
        }
        #endregion
    }
}
