using System.Web.Http;
using Topshelf.Common.Tests;

namespace Topshelf.WebApi.Ninject.Tests.Controllers
{
    public class TestDependencyController : ApiController
    {
        private readonly SampleDependency _dependency;

        public TestDependencyController(SampleDependency dependency)
        {
            _dependency = dependency;
        }

        [HttpGet]
        public int Get()
        {
            if(_dependency != null)
                return 42;
            return 0;
        }
    }
}
