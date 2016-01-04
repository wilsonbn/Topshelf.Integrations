using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Topshelf.WebApi.Tests
{
    public class LocalConstraintAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Debug.WriteLine("ACTION 1 DEBUG pre-processing logging");

            if (!actionContext.RequestContext.IsLocal)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.NotFound,"");
                actionContext.Response = response;
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Debug.WriteLine("ACTION 1 DEBUG  OnActionExecuted Response " + actionExecutedContext.Response.StatusCode.ToString());
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent == null) return;
            var type = objectContent.ObjectType; //type of the returned object
            var value = objectContent.Value; //holding the returned value
        }
    }
}
