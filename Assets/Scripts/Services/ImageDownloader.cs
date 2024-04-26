using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Utils;
using Interfaces;
using Newtonsoft.Json.Linq;
using UI.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
	public class ImageDownloader : IImageDownloader
	{
		private readonly Dictionary<string, Texture2D> _images = new();
		private readonly IFileDownloader _fileDownloader;
		private readonly ICacheService _cacheService;

		public ImageDownloader(IFileDownloader fileDownloader, ICacheService cacheService)
		{
			_fileDownloader = fileDownloader;
			_cacheService = cacheService;
		}
		
		public bool Ready { get; private set; }
		
		public async Task Initialize(IProgressDisplay loading = null)
		{
			this.ThrowIfDependencyNotReady(_fileDownloader);
			this.ThrowIfDependencyNotReady(_cacheService);
			
			loading?.UpdateProgress(1);
			Ready = true;
		}

		public bool TryGetImage(string path, out Texture2D image)
		{
			if (_images.TryGetValue(path, out image))
				return true;

			if (_cacheService.TryRead(GetCacheKey(path), out Stream stream))
			{
				image = LoadFromFile(stream);
				if (image == null)
					return false;
				
				_images.Add(path, image);
				return true;
			}

			return false;
		}
		
		public async Task<Texture2D> TryGetImageAsync(string path)
		{
			if (_images.TryGetValue(path, out var image))
				return image;

			if (_cacheService.TryRead(GetCacheKey(path), out Stream stream))
			{
				image = await LoadFromFileAsync(stream);
				if (image == null)
					return null;
				
				_images.Add(path, image);
				return image;
			}

			return null;
		}
		
		public ILoadImageRequest LoadImage(string path)
		{
			return new LoadImageRequest(this, path);
		}

		public async Task<Texture2D> Download(string path, IProgressDisplay loading = null)
		{
			this.ThrowIfNotReady();

			var hash = await _fileDownloader.GetHash(path);
			var cacheKey = GetCacheKey(path);
			if (_cacheService.HasSameHash(cacheKey, hash))
			{

				Texture2D cachedImage = await TryGetImageAsync(path);
				if (cachedImage != null)
				{
					loading?.UpdateProgress(1);
					return cachedImage;
				}
			}
			
			var pathToImage = await _fileDownloader.Download(path, loading);

			Texture2D texture = await LoadFromFileAsync(File.OpenRead(pathToImage));
			if (texture == null)
			{
				throw new Exception("Image decoding failed");
			}
			_images.Add(path, texture);
			_cacheService.CacheFile(pathToImage, cacheKey, hash);

			return texture;
		}

		private static string GetCacheKey(string path)
		{
			return $"image_{path}";
		}
		
		private Texture2D LoadFromFile(Stream stream)
		{
			using (stream)
			{
				using (MemoryStream ms = new MemoryStream())
				{
					try
					{
						stream.CopyTo(ms);
						var texture = new Texture2D(1, 1);
						texture.LoadImage(ms.ToArray());
						return texture;
					}
					catch (Exception)
					{
						return null;
					}
				}
			}
		}
		
		private async Task<Texture2D> LoadFromFileAsync(Stream stream)
		{
			using (stream)
			{
				using (MemoryStream ms = new MemoryStream())
				{
					try
					{
						await stream.CopyToAsync(ms);
						await UniTask.SwitchToMainThread();
						var texture = new Texture2D(1, 1);
						texture.LoadImage(ms.ToArray());
						return texture;
					}
					catch (Exception)
					{
						return null;
					}
				}
			}
		}
		
		private struct LoadImageRequest : ILoadImageRequest
		{
			private readonly ImageDownloader _owner;
			private readonly string _path;

			public LoadImageRequest(ImageDownloader owner, string path)
			{
				_owner = owner;
				_path = path;
			}

			public Task<Texture2D> FromCache()
			{
				return _owner.TryGetImageAsync(_path);
			}

			public async Task<Texture2D> FromServer(IProgressDisplay loading = null)
			{
				return await _owner.Download(_path, loading);
			}
		}
	}
}