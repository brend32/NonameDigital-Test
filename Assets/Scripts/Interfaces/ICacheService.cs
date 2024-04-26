using System.IO;
using System.Threading.Tasks;

namespace Interfaces
{
	public interface ICacheService : IService
	{
		string CacheFile(string fullPath, string key, string hash);
		bool HasSameHash(string key, string hash);
		string GetPathToCacheFile(string key);
		bool TryRead(string key, out Stream readStream);
	}
}