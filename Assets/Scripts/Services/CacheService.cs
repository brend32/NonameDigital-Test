using System.IO;
using Utils;
using Interfaces;
using UnityEngine;

namespace Services
{
	public class CacheService : ICacheService
	{
		private readonly IPathResolver _pathResolver;

		public CacheService(IPathResolver pathResolver)
		{
			_pathResolver = pathResolver;
		}
		
		public bool Ready => _pathResolver?.Ready == true;
		
		public string CacheFile(string fullPath, string key, string hash)
		{
			this.ThrowIfNotReady();
			
			var path = GetPathToCacheFile(key);
			var pathToHash = GetPathToCacheFileHash(path);
			
			IOUtils.CreateFolders(path);
			IOUtils.DeleteIfExists(path);
			IOUtils.DeleteIfExists(pathToHash);
			
			File.Move(fullPath, path);
			File.WriteAllText(pathToHash, hash);
			
			return path;
		}

		public bool HasSameHash(string key, string hash)
		{
			this.ThrowIfNotReady();
			
			var path = GetPathToCacheFile(key);
			var pathToHash = GetPathToCacheFileHash(path);

			return File.Exists(pathToHash) && File.ReadAllText(pathToHash) == hash;
		}

		public bool TryRead(string key, out Stream readStream)
		{
			this.ThrowIfNotReady();
			
			var path = GetPathToCacheFile(key);

			if (File.Exists(path))
			{
				readStream = File.OpenRead(path);
				return true;
			}

			readStream = default;
			return false;
		}

		public string GetPathToCacheFile(string key)
		{
			return _pathResolver.GetPathToCacheFile(key);
		}
		
		private string GetPathToCacheFileHash(string path)
		{
			return $"{path}.hash";
		}
	}
}