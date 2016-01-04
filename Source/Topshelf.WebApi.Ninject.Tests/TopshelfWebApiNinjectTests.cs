using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Filters;
using NUnit.Framework;
using Topshelf.Common.Tests;
using Topshelf.Ninject;

namespace Topshelf.WebApi.Ninject.Tests
{
    [TestFixture]
    public class TopshelfWebApiNinjectTests
    {
        [Test]
        public void TestWebApiCanInitialize()
        {
            HttpServer server = null;
            string controllerResponse = string.Empty;

            Host host = HostFactory.New(configurator =>
            {
                configurator.UseTestHost();
                configurator.UseNinject(new SampleNinjectModule());

                configurator.Service<SampleNinjectService>(s =>
                {
                    s.ConstructUsingNinject();
                    s.WhenStarted((service, control) => service.Start());
                    s.AfterStartingService(() =>
                    {
                        using (var client = new HttpMessageInvoker(server))
                        using (var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/Api/TestDependency"))
                        using (var response = client.SendAsync(request, CancellationToken.None).Result)
                            controllerResponse = response.Content.ReadAsStringAsync().Result;
                    });
                    s.WhenStopped((service, control) => service.Stop());
                    s.WebApiEndpoint(api =>
                    {
                        //Use an in-memory server instead of the SelfHost server.
                        api.ServerFactory = uri => new HttpServer(new HttpConfiguration());
                        api.ConfigureRoutes(Configure);
                        api.UseNinjectDependencyResolver();
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
                configurator.UseNinject(new SampleNinjectModule());

                configurator.Service<SampleNinjectService>(s =>
                {
                    s.ConstructUsingNinject();
                    s.WhenStarted((service, control) => service.Start());
                    s.AfterStartingService(() =>
                    {
                        using (var client = new HttpMessageInvoker(server))
                        using (var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/Api/TestDependency?blah=blah"))
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
                        api.UseNinjectDependencyResolver();
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
            "Api/TestDependency",
            new { controller = "TestDependency" });
        }
        private static void Filters(HttpFilterCollection filters)
        {
            filters.Add(new LocalConstraintAttribute());
        }
    }
}
