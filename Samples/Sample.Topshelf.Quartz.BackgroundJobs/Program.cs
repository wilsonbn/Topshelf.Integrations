using System;
using Ninject.Modules;
using Quartz;
using Quartz.Impl.Matchers;
using Topshelf;
using Topshelf.Ninject;
using Topshelf.Quartz;
using Topshelf.Quartz.Ninject;

namespace Sample.Topshelf.Quartz.BackgroundJobs
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(c =>
                {
<<<<<<< Updated upstream
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
                                                          JobBuilder.Create<SampleJob>()
                                                                    .WithIdentity("sample")
                                                                    .Build())
                                                 .AddTrigger(() =>
                                                             TriggerBuilder.Create()
                                                                           .WithIdentity("sample")
                                                                           .WithSimpleSchedule(
                                                                               builder =>
                                                                               builder.WithIntervalInSeconds(5)
                                                                                      .RepeatForever()).Build())
                                                 .WithJobListener(() =>
                                                     {
                                                         return new QuartzJobListenerConfig(
                                                             new SimpleJobListener(),
                                                             KeyMatcher<JobKey>.KeyEquals(new JobKey("sample")));
                                                     })
                                                 .WithTriggerListener(() =>
                                                     {
                                                         return new QuartzTriggerListenerConfig(
                                                             new SimpleTriggerListener(),
                                                             KeyMatcher<TriggerKey>.KeyEquals(new TriggerKey("sample")));
                                                     })
                                                     .WithScheduleListener(() => new SimpleScheduleListener())
                                );
                        });
=======
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
                                                  JobBuilder.Create<SampleJob>()
                                                            .WithIdentity("sample")
                                                            .Build())
                                         .AddTrigger(() =>
                                                     TriggerBuilder.Create()
                                                                   .WithIdentity("sample")
                                                                   .WithSimpleSchedule(
                                                                       builder =>
                                                                       builder.WithIntervalInSeconds(5)
                                                                              .RepeatForever()).Build())
                                         .WithJobListener(() =>
                                         {
                                             return new QuartzJobListenerConfig(
                                                 new SimpleJobListener(),
                                                 KeyMatcher<JobKey>.KeyEquals(new JobKey("sample")));
                                         })
                                         .WithTriggerListener(() =>
                                         {
                                             return new QuartzTriggerListenerConfig(
                                                 new SimpleTriggerListener(),
                                                 KeyMatcher<TriggerKey>.KeyEquals(new TriggerKey("sample")));
                                         })
                                             .WithScheduleListener(() => new SimpleScheduleListener())
                        );
>>>>>>> Stashed changes
                });
        }
    }

    public class SimpleScheduleListener : ISchedulerListener
    {
        public void JobScheduled(ITrigger trigger)
        {
            Console.WriteLine("SAMPLE: Job Scheduled from trigger: " + trigger.Key.Name);
        }

        public void JobUnscheduled(TriggerKey triggerKey)
        {
            Console.WriteLine("SAMPLE: Job Unscheduled from trigger: " + triggerKey.Name);
        }

        public void TriggerFinalized(ITrigger trigger)
        {
            Console.WriteLine("SAMPLE: Trigger Finalized from trigger: " + trigger.Key.Name);
        }

        public void TriggerPaused(TriggerKey triggerKey)
        {
            Console.WriteLine("SAMPLE: Trigger Paused from trigger: " + triggerKey.Name);
        }

        public void TriggersPaused(string triggerGroup)
        {
            Console.WriteLine("SAMPLE: Trigger Paused for group: " + triggerGroup);
        }

        public void TriggerResumed(TriggerKey triggerKey)
        {
            Console.WriteLine("SAMPLE: Trigger Paused from trigger: " + triggerKey.Name);
        }

        public void TriggersResumed(string triggerGroup)
        {
            Console.WriteLine("SAMPLE: Trigger Resumed for group: " + triggerGroup);
        }

        public void JobAdded(IJobDetail jobDetail)
        {
            Console.WriteLine("SAMPLE: Job Added: " + jobDetail.Key.Name);
        }

        public void JobDeleted(JobKey jobKey)
        {
            Console.WriteLine("SAMPLE: Job Deleted: " + jobKey.Name);
        }

        public void JobPaused(JobKey jobKey)
        {
            Console.WriteLine("SAMPLE: Job Paused: " + jobKey.Name);
        }

        public void JobsPaused(string jobGroup)
        {
            Console.WriteLine("SAMPLE: Jobs Paused for group: " + jobGroup);
        }

        public void JobResumed(JobKey jobKey)
        {
            Console.WriteLine("SAMPLE: Job Resumed: " + jobKey.Name);
        }

        public void JobsResumed(string jobGroup)
        {
            Console.WriteLine("SAMPLE: Jobs Resumed for group: " + jobGroup);
        }

        public void SchedulerError(string msg, SchedulerException cause)
        {
            Console.WriteLine("SAMPLE: Scheduler error: " + msg + " with exception: " + cause.Message);
        }

        public void SchedulerInStandbyMode()
        {
            Console.WriteLine("SAMPLE: Scheduler is in standby");
        }

        public void SchedulerStarted()
        {
            Console.WriteLine("SAMPLE: Scheduler started");
        }

        public void SchedulerStarting()
        {
            Console.WriteLine("SAMPLE: Scheduler starting...");
        }

        public void SchedulerShutdown()
        {
            Console.WriteLine("SAMPLE: Scheduler shutdown");
        }

        public void SchedulerShuttingdown()
        {
            Console.WriteLine("SAMPLE: Scheduler shutting down...");
        }

        public void SchedulingDataCleared()
        {
            Console.WriteLine("SAMPLE: Scheduling data cleard");
        }
    }

    public class SimpleJobListener : IJobListener
    {
        public void JobToBeExecuted(IJobExecutionContext context)
        {
            Console.WriteLine("SAMPLE: Job is about to execute: " + context.JobDetail.Key.Name);
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            Console.WriteLine("SAMPLE: Job execution vetoed: " + context.JobDetail.Key.Name);
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            Console.WriteLine("SAMPLE: Job was executed: " + context.JobDetail.Key.Name);
        }

        public string Name { get { return "SAMPLE JOB LISTENER"; } }
    }

    public class SimpleTriggerListener : ITriggerListener
    {
        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            Console.WriteLine("SAMPLE: Trigger {0} fired for job {1}", trigger.Key.Name, context.JobDetail.Key.Name);
        }

        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            Console.WriteLine("SAMPLE: Trigger {0} vetoed for job {1}", trigger.Key.Name, context.JobDetail.Key.Name);
            return false;
        }

        public void TriggerMisfired(ITrigger trigger)
        {
            Console.WriteLine("SAMPLE: Trigger {0} misfired", trigger.Key.Name);
        }

        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            Console.WriteLine("SAMPLE: Trigger {0} completed for job {1} with code {2}", trigger.Key.Name, context.JobDetail.Key.Name, triggerInstructionCode);
        }

        public string Name { get { return "SAMPLE TRIGGER LISTENER"; } }
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
