using System;
using Topshelf.Runtime;
using Topshelf.ServiceConfigurators;

namespace Topshelf.WebApi
{
	public static class WebApiServiceConfiguratorExtensions
	{
		public static ServiceConfigurator<T> WebApiEndpoint<T>(this ServiceConfigurator<T> configurator, Action<WebApiConfigurator> webConfigurator) where T : class
		{
			var config = new WebApiConfigurator();

			webConfigurator(config);

			configurator.BeforeStartingService(t => config.Initialize());
			configurator.BeforeStoppingService(t => config.Shutdown());

			return configurator;
		}
	}
}