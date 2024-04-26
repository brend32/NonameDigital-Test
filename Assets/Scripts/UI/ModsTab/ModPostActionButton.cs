using System;
using Data;
using Interfaces;
using Services;
using TMPro;
using UI.Common;
using UnityEngine;
using Zenject;

namespace UI.ModsTab
{
	public class ModPostActionButton : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _text;

		[Inject] private ModsService _modsService;
		[Inject] private IShareService _shareService;
		private DownloadWindow _downloadWindow;

		private ModPostData _data;

		private void Start()
		{
			_downloadWindow = FindObjectOfType<DownloadWindow>(includeInactive: true); 
			// Don't know how to get dependencies from project and scene context
			// Probably it is impossible
		}

		public void UpdateState(ModPostData data)
		{
			_data = data;
			if (_modsService.IsModDownloaded(data))
			{
				_text.text = "Share";
			}
			else
			{
				_text.text = "Download";
			}
		}

		public void Action()
		{
			if (_modsService.IsModDownloaded(_data))
			{
				Share();
			}
			else
			{
				DownloadAndShare();
			}
		}

		private void Share()
		{
			_shareService.ShareFile(_modsService.GetPathToModFile(_data), _data.Title, "");
		}

		private async void DownloadAndShare()
		{
			ModPostData data = _data;
			await _downloadWindow.Download(async (token, progress) =>
			{
				await _modsService.DownloadAndCacheMod(data, progress, token);
			});
			
			if (data != _data)
				return;
			
			UpdateState(_data);
			Share();
		}
	}
}