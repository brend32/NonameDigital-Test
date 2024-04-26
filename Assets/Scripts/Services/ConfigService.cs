using System;
using System.Threading.Tasks;
using Interfaces;
using UI.Common;
using Utils;

namespace Services
{
	public class ConfigService : IService
	{
		private readonly IFileDownloader _fileDownloader;

		public ConfigService(IFileDownloader fileDownloader)
		{
			_fileDownloader = fileDownloader;
		}
		
		public bool Ready { get; private set; }

		public async Task Initialize(IProgressDisplay loading = null)
		{
			this.ThrowIfDependencyNotReady(_fileDownloader);

			// There is no configs to download
			// Faking progress 
			for (int i = 0; i < 10; i++)
			{
				await Task.Delay(50);
				loading?.UpdateProgress(i / 9f);
			}

			Ready = true;
		}
	}
}