using System.IO;
using System.Threading.Tasks;

namespace Interfaces
{
	public interface IShareService : IService
	{
		void ShareFile(string fullPath, string title, string subject);
	}
}