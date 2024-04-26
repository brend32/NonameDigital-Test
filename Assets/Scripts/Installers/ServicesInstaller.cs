using Interfaces;
using Services;
using Zenject;

namespace DefaultNamespace.Installers
{
	public class ServicesInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<IPathResolver>().To<PathResolverService>().AsSingle();
			Container.Bind<ICacheService>().To<CacheService>().AsSingle();
			Container.Bind<IFileDownloader>().To<DropboxFileDownloader>().AsSingle();
			Container.Bind<IImageDownloader>().To<ImageDownloader>().AsSingle();
			Container.Bind<IShareService>().To<ShareService>().AsSingle();
			Container.Bind<ModsService>().AsSingle();
			Container.Bind<ConfigService>().AsSingle();
			Container.Bind<SceneLoader>().AsSingle();
		}
	}
}