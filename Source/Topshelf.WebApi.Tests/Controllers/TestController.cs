using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Topshelf.WebApi.Tests.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public int Get()
        {
            return 42;
        }

        [LocalConstraint]
        [HttpGet]
        public int Get(string blah)
        {
            return 42;
        }
    }
}
