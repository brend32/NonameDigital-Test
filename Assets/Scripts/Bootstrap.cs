using System;
using Interfaces;
using Services;
using UI.Common;
using UnityEngine;
using Utils;
using Zenject;

public class Bootstrap : MonoBehaviour
{
	[SerializeField] private LoadingWithProgress _progressBar;
	[SerializeField] private GameObject _errorMessage;
	[SerializeField] private GameObject _loadingPanel;
		
	[Inject] private IFileDownloader _fileDownloader;
	[Inject] private IImageDownloader _imageDownloader;
	[Inject] private ModsService _modsService;
	[Inject] private ConfigService _configService;
	[Inject] private SceneLoader _sceneLoader;

	private void Start()
	{
		_errorMessage.gameObject.SetActive(false);
		
		Initialize();
	}

	public async void Initialize()
	{
		try
		{
			var progressAggregator = new GroupProgressAggregator(_progressBar, 5);
			
			await _fileDownloader.Initialize(progressAggregator.GetNextSource());
			await _imageDownloader.Initialize(progressAggregator.GetNextSource());

			await _configService.Initialize(progressAggregator.GetNextSource());
			await _modsService.Initialize(progressAggregator.GetNextSource());
				
			var loadingOperation = _sceneLoader.Load(SceneLoader.Home, progressAggregator.GetNextSource());
			await loadingOperation.WaitWithActivation();
			loadingOperation.SetAsActiveScene();

			HideBootstrapScene();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
			DisplayErrorMessage();
			throw;
		}
	}

	public void CloseApp()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}

	private void HideBootstrapScene()
	{
		_sceneLoader.Unload(gameObject.scene.name);
	}

	private void DisplayErrorMessage()
	{
		_errorMessage.gameObject.SetActive(true);
		_loadingPanel.gameObject.SetActive(false);
	}
}