using Ninject.Modules;
using Quartz;

namespace Ninject.Extensions.Quartz
{
	public class QuartzNinjectModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ISchedulerFactory>().To<NinjectSchedulerFactory>();
			Bind<IScheduler>().ToMethod(ctx => ctx.Kernel.Get<ISchedulerFactory>().GetScheduler()).InSingletonScope();   
		}
	}
}