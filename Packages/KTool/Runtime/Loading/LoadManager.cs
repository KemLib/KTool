using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KTool.Attribute;

namespace KTool.Loading
{
	public class LoadManager : MonoBehaviour
	{
		#region Properties
		private const string LOAD_SCENE_TASK_NAME_FORMAT = "Load scene: {0}";
		private const float LOAD_SCENE_MAX_PROGRESS = 0.4f;

		[SerializeField] private LoadUi defaultLoadUi;

		public ILoadUi LoadUi => ILoadUi.Instance == null ? defaultLoadUi : ILoadUi.Instance;
		#endregion

		#region Unity Event		
		// Start is called before the first frame update
		void Start()
		{
			LoadItem(LoadUi, GetSceneActive(), 0);
		}

		// Update is called once per frame
		void Update()
		{

		}
		#endregion

		#region Load Scene
		public void LoadScene(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
		{
			StartCoroutine(IE_LoadScene(LoadUi, sceneName));
		}
		public void LoadScene(int sceneindex, LoadSceneMode sceneMode = LoadSceneMode.Single)
		{
			StartCoroutine(IE_LoadScene(LoadUi, sceneindex));
		}
		private IEnumerator IE_LoadScene(ILoadUi loadUi, string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
		{
			if (!loadUi.IsShow)
			{
				loadUi.Progress = 0;
				loadUi.TaskName = string.Empty;
				loadUi.Show();
			}
			while (loadUi.IsChanging)
				yield return new WaitForEndOfFrame();
			//
			loadUi.TaskName = string.Format(LOAD_SCENE_TASK_NAME_FORMAT, sceneName);
			AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, sceneMode);
			ao.allowSceneActivation = true;
			//
			while (!ao.isDone)
			{
				loadUi.Progress = LOAD_SCENE_MAX_PROGRESS * ao.progress;
				yield return new WaitForEndOfFrame();
			}
			//
			loadUi.TaskName = string.Empty;
			LoadItem(loadUi, GetScene(sceneName), LOAD_SCENE_MAX_PROGRESS);
		}
		private IEnumerator IE_LoadScene(ILoadUi loadUi, int sceneIndex, LoadSceneMode sceneMode = LoadSceneMode.Single)
		{
			if (!loadUi.IsShow)
			{
				loadUi.Progress = 0;
				loadUi.TaskName = string.Empty;
				loadUi.Show();
			}
			while (loadUi.IsChanging)
				yield return new WaitForEndOfFrame();
			//
			loadUi.TaskName = string.Format(LOAD_SCENE_TASK_NAME_FORMAT, sceneIndex);
			AsyncOperation ao = SceneManager.LoadSceneAsync(sceneIndex, sceneMode);
			ao.allowSceneActivation = true;
			//
			while (!ao.isDone)
			{
				loadUi.Progress = LOAD_SCENE_MAX_PROGRESS * ao.progress;
				yield return new WaitForEndOfFrame();
			}
			//
			loadUi.TaskName = string.Empty;
			LoadItem(loadUi, GetScene(sceneIndex), LOAD_SCENE_MAX_PROGRESS);
		}
		#endregion

		#region Load Item
		private void LoadItem(ILoadUi loadUi, Scene scene, float originProgress = 0)
		{
			List<ILoader> items = GetComponents<ILoader>(scene);
			if (items.Count == 0)
			{
				if (loadUi.IsShow)
				{
					loadUi.Progress = 1;
					loadUi.TaskName = string.Empty;
					loadUi.Hide();
				}
			}
			else
			{
				StartCoroutine(IE_GameLoad(loadUi, items, originProgress));
			}
		}
		private IEnumerator IE_GameLoad(ILoadUi loadUi, List<ILoader> items, float originProgress = 0)
		{
			if (!loadUi.IsShow)
			{
				loadUi.Progress = originProgress;
				loadUi.TaskName = string.Empty;
				loadUi.Show();
			}
			while (loadUi.IsChanging)
				yield return new WaitForEndOfFrame();
			//
			float currentProgress = originProgress,
				maxProgress = (1 - currentProgress) / 2,
				unitProgress = maxProgress / items.Count;
			//
			foreach (ILoader item in items)
			{
				Entri entri = new Entri();
				item.GameLoad(entri);
				//
				while (!entri.IsDone)
				{
					loadUi.Progress = currentProgress + unitProgress * entri.Progress;
					loadUi.TaskName = entri.Name;
					yield return new WaitForEndOfFrame();
				}
				//
				currentProgress += unitProgress;
				loadUi.Progress = currentProgress;
				loadUi.TaskName = string.Empty;
			}
			//
			StartCoroutine(IE_GameInit(loadUi, items, currentProgress));
		}
		private IEnumerator IE_GameInit(ILoadUi loadUi, List<ILoader> items, float originProgress = 0)
		{
			if (!loadUi.IsShow)
			{
				loadUi.Progress = originProgress;
				loadUi.TaskName = string.Empty;
				loadUi.Show();
			}
			while (loadUi.IsChanging)
				yield return new WaitForEndOfFrame();
			//
			float currentProgress = originProgress,
				maxProgress = 1 - currentProgress,
				unitProgress = maxProgress / items.Count;
			//
			foreach (ILoader item in items)
			{
				Entri entri = new Entri();
				item.GameInit(entri);
				//
				while (!entri.IsDone)
				{
					loadUi.Progress = currentProgress + unitProgress * entri.Progress;
					loadUi.TaskName = entri.Name;
					yield return new WaitForEndOfFrame();
				}
				//
				currentProgress += unitProgress;
				loadUi.Progress = currentProgress;
				loadUi.TaskName = string.Empty;
			}
			StartCoroutine(IE_GameStart(loadUi, items));
		}
		private IEnumerator IE_GameStart(ILoadUi loadUi, List<ILoader> items)
		{
			if (loadUi.IsShow)
			{
				loadUi.Progress = 1;
				loadUi.TaskName = string.Empty;
				loadUi.Hide();
			}
			while (loadUi.IsChanging)
				yield return new WaitForEndOfFrame();
			//
			foreach (ILoader item in items)
				item.GameStart();
		}
		#endregion

		#region Utillity
		public List<T> GetComponents<T>(Scene scene)
		{
			List<T> components = new List<T>();
			if (!scene.IsValid() || !scene.isLoaded)
				return components;
			GameObject[] rootGO = scene.GetRootGameObjects();
			foreach (GameObject go in rootGO)
			{
				T[] items = go.GetComponentsInChildren<T>();
				if (items.Length == 0)
					continue;
				components.AddRange(items);
			}
			return components;
		}
		public Scene GetScene(string sceneName)
		{
			return SceneManager.GetSceneByName(sceneName);
		}
		public Scene GetScene(int sceneIndex)
		{
			return SceneManager.GetSceneByBuildIndex(sceneIndex);
		}
		public Scene GetSceneActive()
		{
			return SceneManager.GetActiveScene();
		}
		#endregion
	}
}
