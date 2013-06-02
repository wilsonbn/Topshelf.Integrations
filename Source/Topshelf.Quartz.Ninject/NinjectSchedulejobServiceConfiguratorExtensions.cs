using System;
using Ninject;
using Quartz;
using Topshelf.Logging;
using Topshelf.Ninject;
using Topshelf.ServiceConfigurators;

namespace Topshelf.Quartz.Ninject
{
	public static class NinjectScheduleJobServiceConfiguratorExtensions
	{
		public static ServiceConfigurator<T> UseQuartzNinject<T>(this ServiceConfigurator<T> configurator)
			where T : class
		{
			SetupNinject();

			return configurator;
		}

		internal static void SetupNinject()
		{
			var log = HostLogger.Get(typeof(NinjectScheduleJobServiceConfiguratorExtensions));

			IKernel kernel = NinjectBuilderConfigurator.Kernel;

			if (kernel == null)
				throw new Exception("You must call UseNinject() to use the Quartz Topshelf Ninject integration.");

			Func<IScheduler> schedulerFactory = () => kernel.Get<IScheduler>();

			ScheduleJobServiceConfiguratorExtensions.SchedulerFactory = schedulerFactory;

			log.Info("[Topshelf.Quartz.Ninject] Quartz configured to construct jobs with Ninject.");
		}
	}
}
