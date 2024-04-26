using System;
using System.Threading;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace UI.Common
{
	[RequireComponent(typeof(RawImage))]
	public class NetworkImage : MonoBehaviour
	{
		[SerializeField] private LoadingWithProgress _progress;
		[SerializeField] private string _path;
		private RawImage _image;

		[Inject] private IImageDownloader _imageDownloader;

		private string _key;
		private CancellationTokenSource _cancellationTokenSource;

		public UnityEvent<Exception> OnError;
		public UnityEvent OnLoadStarted;
		public UnityEvent<Texture2D> OnImageUpdated;
		public RawImage Image => _image;

		private void Awake()
		{
			_image = GetComponent<RawImage>();
			if (_progress != null)
				_progress.gameObject.SetActive(false);
		}

		private void Start()
		{
			if (string.IsNullOrEmpty(_path) == false)
				Load(_path);
		}

		public virtual void UpdateImage(Texture2D texture)
		{
			if (texture == null)
				return;
			
			if (_progress != null)
				_progress.gameObject.SetActive(false);
			_image.texture = texture;
			OnImageUpdated.Invoke(texture);
		}

		public async void Load(string path)
		{
			if (_cancellationTokenSource != null && path == _key)
				return;
			
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			_key = path;

			CancellationToken token = _cancellationTokenSource.Token;
			try
			{
				ILoadImageRequest request = _imageDownloader.LoadImage(path);
				
				Texture2D image = await request.FromCache();
				if (token.IsCancellationRequested)
					return;
				
				OnLoadStarted.Invoke();
				if (_progress != null)
					_progress.gameObject.SetActive(true);
				
				UpdateImage(image);
				
				image = await request.FromServer(_progress);
				if (token.IsCancellationRequested)
					return;
				
				UpdateImage(image);
				
				_cancellationTokenSource.Dispose();
				_cancellationTokenSource = null;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				if (_progress != null)
					_progress.gameObject.SetActive(false);
				OnError.Invoke(e);
			}
		}
	}
}