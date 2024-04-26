using System.Threading;
using System.Threading.Tasks;
using UI.Common;

namespace Interfaces
{
	public interface IFileDownloader : IService
	{
		public Task<string> Download(string path, IProgressDisplay loading = null, CancellationToken cancellationToken = default);
		public Task<string> GetHash(string path, CancellationToken cancellationToken = default);
		Task Initialize(IProgressDisplay loading = null);
	}
}