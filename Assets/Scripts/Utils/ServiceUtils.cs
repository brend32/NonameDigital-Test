using System;
using Interfaces;

namespace Utils
{
	public static class ServiceUtils
	{
		public static void ThrowIfNotReady(this IService service)
		{
			if (service.Ready == false)
				throw new Exception($"Service not ready {service.GetType().FullName}");
		}
		
		public static void ThrowIfDependencyNotReady(this IService service, IService dependency)
		{
			if (dependency.Ready == false)
				throw new Exception($"Can`t perform action {service.GetType().FullName} dependency ({dependency.GetType().FullName}) not ready");
		}
	}
}