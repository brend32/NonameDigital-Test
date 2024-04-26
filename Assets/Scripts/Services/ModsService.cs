using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Interfaces;
using Newtonsoft.Json;
using UI.Common;
using Utils;

namespace Services
{
	public class ModsService : IService
	{
		private LoadingWithProgress _loading;
		private readonly IFileDownloader _fileDownloader;
		private readonly ICacheService _cacheService;
		private bool _initializing;

		public ModsService(IFileDownloader fileDownloader, ICacheService cacheService)
		{
			_fileDownloader = fileDownloader;
			_cacheService = cacheService;
		}
		
		public ModsData Data { get; private set; }
		public bool Ready { get; private set; }

		public async Task Initialize(IProgressDisplay progressDisplay = null)
		{
			this.ThrowIfDependencyNotReady(_fileDownloader);
			this.ThrowIfDependencyNotReady(_cacheService);
			_initializing = true;
			
			await UpdateMods(progressDisplay);

			_initializing = false;
			Ready = true;
		}

		public async Task UpdateMods(IProgressDisplay progressDisplay = null)
		{
			if (_initializing == false)
				this.ThrowIfNotReady();

			var path = await _fileDownloader.Download("mods.json", progressDisplay);
			var text = await File.ReadAllTextAsync(path);
			Data = JsonConvert.DeserializeObject<ModsData>(text);

			if (Data.Categories.Contains("All") == false)
			{
				Data.Categories.Insert(0, "All");
			}
		}

		public async Task<string> DownloadAndCacheMod(ModPostData data, IProgressDisplay progressDisplay = null, CancellationToken cancellationToken = default)
		{
			this.ThrowIfNotReady();

			var hash = await _fileDownloader.GetHash(data.FilePath, cancellationToken);
			var cacheKey = GetModCacheKey(data.FilePath);
			var pathToCacheFile = _cacheService.GetPathToCacheFile(cacheKey);
			if (_cacheService.HasSameHash(cacheKey, hash))
				return pathToCacheFile;

			var path = await _fileDownloader.Download(data.FilePath, progressDisplay, cancellationToken);
			cancellationToken.ThrowIfCancellationRequested();

			return _cacheService.CacheFile(path, cacheKey, hash);
		}

		public string GetPathToModFile(ModPostData data)
		{
			this.ThrowIfNotReady();
			
			var cacheKey = GetModCacheKey(data.FilePath);
			var pathToCacheFile = _cacheService.GetPathToCacheFile(cacheKey);
			if (File.Exists(pathToCacheFile) == false)
				throw new FileNotFoundException(data.FilePath);
			
			return pathToCacheFile;
		}

		public bool IsModDownloaded(ModPostData data)
		{
			this.ThrowIfNotReady();

			var cacheKey = GetModCacheKey(data.FilePath);
			var pathToCacheFile = _cacheService.GetPathToCacheFile(cacheKey);
			return File.Exists(pathToCacheFile);
		}

		private static string GetModCacheKey(string path)
		{
			return $"mod_{path}";
		}
	}
}