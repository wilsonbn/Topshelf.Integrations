using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace Topshelf.Common.Tests
{
    public class SampleNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISampleDependency>().To<SampleDependency>();
        }
    }
}

