using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DefaultNamespace;
using Utils;
using Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UI.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
	public class DropboxFileDownloader : IFileDownloader
	{
		private readonly IPathResolver _pathResolver;
		private string _sessionToken;

		public DropboxFileDownloader(IPathResolver pathResolver)
		{
			_pathResolver = pathResolver;
		}

		public bool Ready { get; private set; }

		public async Task Initialize(IProgressDisplay loading = null)
		{
			this.ThrowIfDependencyNotReady(_pathResolver);
			
			var form = new WWWForm();
			form.AddField("grant_type", "refresh_token");
			form.AddField("refresh_token", AppSecrets.DropboxAppRefreshToken);

			var base64Authorization = Convert.ToBase64String(
				Encoding.ASCII.GetBytes($"{AppSecrets.DropboxAppKey}:{AppSecrets.DropboxAppSecret}")
				);

			using var request = UnityWebRequest.Post("https://api.dropbox.com/oauth2/token", form);
			request.SetRequestHeader("Authorization", $"Basic {base64Authorization}");

			var sendRequest = request.SendWebRequest();

			while (sendRequest.isDone == false)
			{
				loading?.UpdateProgress(sendRequest.progress);
				await Task.Yield();
			}

			if (request.result != UnityWebRequest.Result.Success)
			{
				throw new Exception(request.error);
			}

			var data = JObject.Parse(request.downloadHandler.text);
			_sessionToken = data["access_token"]!.Value<string>();
			loading?.UpdateProgress(1);
			
			Ready = true;
		}

		public async Task<string> Download(string path, IProgressDisplay loading = null, CancellationToken cancellationToken = default)
		{
			this.ThrowIfNotReady();
			
			UnityWebRequest downloadRequest = GetRequestForFileDownload(path);

			var operation = downloadRequest.SendWebRequest();
			while (operation.isDone == false)
			{
				loading?.UpdateProgress(operation.progress);
				await Task.Delay(100, cancellationToken);

				if (cancellationToken.IsCancellationRequested)
				{
					downloadRequest.Abort();
					throw new TaskCanceledException();
				}
			}

			if (downloadRequest.result != UnityWebRequest.Result.Success)
			{
				throw new Exception(downloadRequest.error);
			}
			
			var filePath = _pathResolver.GetPathToTemporaryFile(path);
			new FileInfo(filePath).Directory?.Create();
			
			await File.WriteAllBytesAsync(filePath, downloadRequest.downloadHandler.data, cancellationToken);

			return filePath;
		}

		public async Task<string> GetHash(string path, CancellationToken cancellationToken = default)
		{
			this.ThrowIfNotReady();
			
			UnityWebRequest downloadRequest = GetRequestForFileMetadata(path);

			var operation = downloadRequest.SendWebRequest();
			while (operation.isDone == false)
			{
				await Task.Yield();
				if (cancellationToken.IsCancellationRequested)
				{
					downloadRequest.Abort();
					throw new OperationCanceledException();
				}
			}

			if (downloadRequest.result != UnityWebRequest.Result.Success)
			{
				throw new Exception(downloadRequest.error);
			}

			var response = JsonConvert.DeserializeObject<JObject>(downloadRequest.downloadHandler.text);
			if (response?["content_hash"] == null)
			{
				throw new Exception($"Invalid response: \n{downloadRequest.downloadHandler.text}");
			}
			
			return response["content_hash"].Value<string>();
		}
		
		private UnityWebRequest GetRequestForFileDownload(string relativePathToFile)
		{
			var startingSlash = relativePathToFile[0] == '/' ? "" : "/";
			
			var request = UnityWebRequest.Get("https://content.dropboxapi.com/2/files/download");
			request.SetRequestHeader("Authorization", $"Bearer {_sessionToken}");
			request.SetRequestHeader("Dropbox-API-Arg", $"{{\"path\": \"{startingSlash}{relativePathToFile}\"}}");
			return request;
		}
		
		private UnityWebRequest GetRequestForFileMetadata(string relativePathToFile)
		{
			var startingSlash = relativePathToFile[0] == '/' ? "" : "/";

			var postData = $"{{\"include_deleted\": false,\"include_has_explicit_shared_members\": false,\"include_media_info\": false,\"path\": \"{startingSlash}{relativePathToFile}\"}}";
			var request = UnityWebRequest.Post(
				"https://api.dropboxapi.com/2/files/get_metadata", 
				postData, 
				"application/json");
			request.SetRequestHeader("Authorization", $"Bearer {_sessionToken}");
			return request;
		}
	}
}