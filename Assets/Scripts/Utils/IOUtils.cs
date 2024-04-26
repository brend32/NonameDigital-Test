using System.IO;

namespace Utils
{
	public static class IOUtils
	{
		public static void CreateFolders(string pathToFile)
		{
			new FileInfo(pathToFile).Directory?.Create();
		}

		public static void DeleteIfExists(string pathToFile)
		{
			if (File.Exists(pathToFile))
				File.Delete(pathToFile);
		}
	}
}