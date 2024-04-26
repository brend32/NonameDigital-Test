using System;
using System.Threading;
using System.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace UI.Common
{
	public class DownloadWindow : MonoBehaviour
	{
		[SerializeField] private LoadingWithProgress _progress;
		[SerializeField] private ErrorWindow _errorWindow;

		private CancellationTokenSource _cancellationTokenSource;

		public IProgressDisplay Progress => _progress;

		public async Task Download(Func<CancellationToken, IProgressDisplay, Task> request)
		{
			gameObject.SetActive(true);
			_cancellationTokenSource?.Cancel();

			_cancellationTokenSource = new CancellationTokenSource();
			_progress.ResetProgress();
			try
			{
				await request(_cancellationTokenSource.Token, _progress);
			}
			catch (TaskCanceledException)
			{
				throw;
			}
			catch (Exception)
			{
				if (_errorWindow != null)
					_errorWindow.Show();
				throw;
			}
			finally
			{
				_cancellationTokenSource.Dispose();
				_cancellationTokenSource = null;
				gameObject.SetActive(false);
			}
		}

		public void Cancel()
		{
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = null;
			gameObject.SetActive(false);
		}
	}
}