using System.IO;
using Interfaces;
using UnityEngine;
using Utils;

namespace Services
{
	public class PathResolverService : IPathResolver
	{
		public bool Ready => true;
		public string CacheFolderPath => Application.temporaryCachePath;
		public string TemporaryFolderPath => Application.temporaryCachePath;

		public string GetPathToCacheFile(string key)
		{
			return Path.Combine(CacheFolderPath, key.MD5());
		}
		
		public string GetPathToTemporaryFile(string name)
		{
			return Path.Combine(CacheFolderPath, name);
		}
	}
}