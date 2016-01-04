using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Filters;
using NUnit.Framework;
using Topshelf.Common.Tests;

namespace Topshelf.WebApi.Tests
{
    [TestFixture]
    public class TopshelfWebApiTests
    {
        [Test]
        public void TestWebApiCanInitialize()
        {
            HttpServer server = null;
            string controllerResponse = string.Empty;

            Host host = HostFactory.New(configurator =>
            {
                configurator.UseTestHost();

                configurator.Service<SampleService>(s =>
                {
                    s.ConstructUsing(settings => new SampleService());
                    s.WhenStarted((service, control) => service.Start());
                    s.AfterStartingService(() =>
                    {
                        using (var client = new HttpMessageInvoker(server))
                        using (var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/Api/Test"))
                        using (var response = client.SendAsync(request, CancellationToken.None).Result)
                            controllerResponse = response.Content.ReadAsStringAsync().Result;
                    });
                    s.WhenStopped((service, control) => service.Stop());
                    s.WebApiEndpoint(api =>
                    {
                        //Use an in-memory server instead of the SelfHost server.
                        api.ServerFactory = uri => new HttpServer(new HttpConfiguration());
                        api.ConfigureRoutes(Configure);
                        server = api.Build();
                    });
                });
            });

            host.Run();

            Assert.AreEqual("42", controllerResponse);
        }

        [Test]
        public void TestWebApiWithFilterCanInitialize()
        {
            HttpServer server = null;
            string controllerResponse = string.Empty;
            HttpStatusCode controllerStatusCode = (HttpStatusCode)0;

            Host host = HostFactory.New(configurator =>
            {
                configurator.UseTestHost();

                configurator.Service<SampleService>(s =>
                {
                    s.ConstructUsing(settings => new SampleService());
                    s.WhenStarted((service, control) => service.Start());
                    s.AfterStartingService(() =>
                    {
                        using (var client = new HttpMessageInvoker(server))
                        using (var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/Api/Test?blah=blah"))
                        using (var response = client.SendAsync(request, CancellationToken.None).Result)
                        {
                            controllerStatusCode = response.StatusCode;
                            controllerResponse = response.Content.ReadAsStringAsync().Result;
                        }
                    });
                    s.WhenStopped((service, control) => service.Stop());
                    s.WebApiEndpoint(api =>
                    {
                        //Use an in-memory server instead of the SelfHost server.
                        api.ServerFactory = uri => new HttpServer(new HttpConfiguration());
                        api.ConfigureRoutes(Configure);
                        api.ConfigureFilters(Filters);
                        server = api.Build();
                    });
                });
            });

            host.Run();

            //The in-memory server will not be seen as IsLocal by Filter
            Assert.AreEqual(HttpStatusCode.NotFound, controllerStatusCode);
            Assert.AreNotEqual("42", controllerResponse);
        }

        private void Configure(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
            "DefaultApiWithId",
            "Api/Test",
            new { controller = "Test" });
        }

        private static void Filters(HttpFilterCollection filters)
        {
            filters.Add(new LocalConstraintAttribute());
        }
    }
}
