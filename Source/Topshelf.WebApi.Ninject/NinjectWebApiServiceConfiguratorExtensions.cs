using System;
using Ninject;
using Topshelf.Logging;
using Topshelf.Ninject;

namespace Topshelf.WebApi.Ninject
{
	public static class NinjectWebApiServiceConfiguratorExtensions
	{
		public static WebApiConfigurator UseNinjectDependencyResolver(this WebApiConfigurator configurator)
		{
			var log = HostLogger.Get(typeof(NinjectWebApiServiceConfiguratorExtensions));
			
			IKernel kernel = NinjectBuilderConfigurator.Kernel;

			if(kernel == null)
				throw new Exception("You must call UseNinject() to use the WebApi Topshelf Ninject integration.");

			configurator.UseDependencyResolver(new NinjectDependencyResolver(kernel));

			log.Info("[Topshelf.WebApi.Ninject] WebAPI Dependency Resolver configured to use Ninject.");

			return configurator;
		}
	}
}