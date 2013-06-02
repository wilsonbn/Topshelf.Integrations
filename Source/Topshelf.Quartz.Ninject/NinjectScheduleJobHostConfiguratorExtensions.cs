using Topshelf.HostConfigurators;

namespace Topshelf.Quartz.Ninject
{
	public static class NinjectScheduleJobHostConfiguratorExtensions
	{
		public static HostConfigurator UseQuartzNinject(this HostConfigurator configurator)
		{
			NinjectScheduleJobServiceConfiguratorExtensions.SetupNinject();

			return configurator;
		}
	}
}