using System;
using Ninject.Modules;
using Topshelf;
using Topshelf.Ninject;

namespace Sample.Topshelf.Ninject
{
    class Program
    {
        static void Main()
        {
            HostFactory.Run(c =>
            {
                c.UseNinject(new SampleModule()); //Initiates Ninject and consumes Modules

                c.Service<SampleService>(s =>
                {
                    //Specifies that Topshelf should delegate to Ninject for construction
                    s.ConstructUsingNinject(); 
                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());
                });
            });
        }
    }

    public class SampleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISampleDependency>().To<SampleDependency>();
        }
    }

    public class SampleService
    {
        private readonly ISampleDependency _sample;

        public SampleService(ISampleDependency sample)
        {
            _sample = sample;
        }

        public bool Start()
        {
            Console.WriteLine("Sample Service Started.");
            Console.WriteLine("Sample Dependency: {0}", _sample);
            return _sample != null;
        }

        public bool Stop()
        {
            return _sample != null;
        }
    }

    public interface ISampleDependency
    {
    }

    public class SampleDependency : ISampleDependency
    {
    }
}
