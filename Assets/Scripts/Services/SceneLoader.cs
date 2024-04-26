using System.Threading.Tasks;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
	public class SceneLoader : IService
	{
		public const string Home = "Home";
		
		public bool Ready => true;

		public LoadingOperation Load(string name, IProgressDisplay progressDisplay = null)
		{
			return new LoadingOperation(SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive), name, progressDisplay);
		}

		public UnloadOperation Unload(string name)
		{
			return new UnloadOperation(SceneManager.UnloadSceneAsync(name));
		}
		
		public readonly struct UnloadOperation
		{
			private readonly AsyncOperation _operation;

			public UnloadOperation(AsyncOperation operation)
			{
				_operation = operation;
			}

			public async Task Wait()
			{
				while (_operation.isDone == false)
				{
					await Task.Yield();
				}
			}
		}
		
		public readonly struct LoadingOperation
		{
			private readonly AsyncOperation _operation;
			private readonly string _sceneName;
			private readonly IProgressDisplay _progressDisplay;

			public LoadingOperation(AsyncOperation operation, string sceneName, IProgressDisplay progressDisplay = null)
			{
				_operation = operation;
				_sceneName = sceneName;
				_progressDisplay = progressDisplay;
			}

			public async Task WaitWithoutActivation()
			{
				_operation.allowSceneActivation = false;

				while (_operation.progress < 0.9)
				{
					_progressDisplay?.UpdateProgress(_operation.progress);
					await Task.Yield();
				}
			}

			public async Task WaitWithActivation()
			{
				while (_operation.isDone == false)
				{
					_progressDisplay?.UpdateProgress(_operation.progress);
					await Task.Yield();
				}
				
				_progressDisplay?.UpdateProgress(1);
			}

			public void Activate()
			{
				_operation.allowSceneActivation = true;
				_progressDisplay?.UpdateProgress(1);
			}

			public void SetAsActiveScene()
			{
				SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
			}
		}
	}
}