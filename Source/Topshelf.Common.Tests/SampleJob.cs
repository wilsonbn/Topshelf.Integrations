using Quartz;

namespace Topshelf.Common.Tests
{
    public class SampleJob : IJob
    {
        public static bool HasRun = false;

        protected SampleDependency Dependency;

        public SampleJob()
        {
            Dependency = new SampleDependency();
        }

        public void Execute(IJobExecutionContext context)
        {
            Dependency.DoWork();
            HasRun = true;
        }
    }

    public class SampleNinjectJob : SampleJob
    {
        public SampleNinjectJob(SampleDependency dependency)
        {
            Dependency = dependency;
        }
    }


    

}