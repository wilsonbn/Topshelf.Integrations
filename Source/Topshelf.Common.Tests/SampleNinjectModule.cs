using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using Ninject.Syntax;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Topshelf.Common.Tests
{
    public class SampleNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISampleDependency>().To<SampleDependency>().InSingletonScope();
            Bind<SampleNinjectJob>().ToSelf().InSingletonScope().WithConstructorArgument("dependency", Kernel.Get<ISampleDependency>());
            Bind<SampleJob>().ToSelf().InSingletonScope();
            Bind<ISchedulerFactory>().To<StdSchedulerFactory>().InSingletonScope();
            Bind<IJobFactory>().To<NinjectJobFactory>().InSingletonScope();
            Bind<IScheduler>().ToMethod((c) =>
            {
                var scheduleFactory = (ISchedulerFactory)c.Kernel.GetService(typeof(ISchedulerFactory));
                var jobFactory = (IJobFactory)c.Kernel.GetService(typeof(IJobFactory));

                IScheduler scheduler = scheduleFactory.GetScheduler();
                scheduler.JobFactory = jobFactory;

                return scheduler;
            }).InSingletonScope();
            Kernel.Get<IScheduler>();
        }
    }

    public class NinjectJobFactory : IJobFactory
    {
        private readonly IResolutionRoot resolutionRoot;

        public NinjectJobFactory(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)this.resolutionRoot.Get(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            this.resolutionRoot.Release(job);
        }
    }

}

