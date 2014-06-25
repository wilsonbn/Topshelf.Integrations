using System;
using Ninject.Modules;
using Quartz;
using Topshelf;
using Topshelf.Ninject;
using Topshelf.Quartz;
using Topshelf.Quartz.Ninject;

namespace Sample.Topshelf.Quartz.BackgroundJobs
{
    class Program
    {
        static void Main()
        {
            HostFactory.Run(c =>
            {
                c.UseNinject(new SampleModule());

                c.Service<SampleService>(s =>
                {
                    // Topshelf.Quartz (Optional) - Construct service using Ninject
                    s.ConstructUsingNinject();

                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());

                    // Topshelf.Quartz.Ninject (Optional) - Construct IJob instance with Ninject
                    s.UseQuartzNinject(); 

                    // Schedule a job to run in the background every 5 seconds.
                    // The full Quartz Builder framework is available here.
                    s.ScheduleQuartzJob(q =>
                        q.WithJob(() =>
                            JobBuilder.Create<SampleJob>().Build())
                            .AddTrigger(() =>
                                TriggerBuilder.Create()
                                    .WithSimpleSchedule(builder => builder.WithIntervalInSeconds(5).RepeatForever()).Build())
                        );
                });
            });
        }
    }

    public class SampleJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("The current time is: {0}", DateTime.Now);
        }
    }

    public class SampleService
    {
        public bool Start()
        {
            Console.WriteLine("Sample Service Started...");
            return true;
        }

        public bool Stop()
        {
            return true;
        }
    }

    public class SampleModule : NinjectModule
    {
        public override void Load()
        {
            
        }
    }
}
