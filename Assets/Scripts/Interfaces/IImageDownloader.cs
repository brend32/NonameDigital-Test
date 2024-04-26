using System;
using System.Threading.Tasks;
using UI.Common;
using UnityEngine;

namespace Interfaces
{
	public interface IImageDownloader : IService
	{
		bool TryGetImage(string path, out Texture2D image);
		Task<Texture2D> TryGetImageAsync(string path);
		Task<Texture2D> Download(string path, IProgressDisplay loading = null);
		ILoadImageRequest LoadImage(string path);
		Task Initialize(IProgressDisplay loading = null);
	}

	public interface ILoadImageRequest
	{
		Task<Texture2D> FromCache();
		Task<Texture2D> FromServer(IProgressDisplay loading = null);
	}
}