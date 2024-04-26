using System.IO;
using Utils;
using Interfaces;

namespace Services
{
	public class ShareService : IShareService
	{
		public bool Ready => true;
		
		public void ShareFile(string fullPath, string title, string subject)
		{
			new NativeShare()
				.AddFile(fullPath)
				.SetTitle(title)
				.SetSubject(subject)
				.Share();
		}
	}
}