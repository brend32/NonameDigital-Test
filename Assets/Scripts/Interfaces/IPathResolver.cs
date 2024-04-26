namespace Interfaces
{
	public interface IPathResolver : IService
	{
		string GetPathToCacheFile(string key);
		string GetPathToTemporaryFile(string name);
	}
}