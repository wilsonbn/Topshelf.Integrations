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

			config.Build();

			configurator.BeforeStartingService(t => config.Server.OpenAsync().Wait());
			configurator.BeforeStoppingService(t => config.Server.CloseAsync().Wait());

			return configurator;
		}
	}
}