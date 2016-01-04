using System;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;
using System.Web.Http.SelfHost;
using Topshelf.Logging;

namespace Topshelf.WebApi
{
    public class WebApiConfigurator
    {
        public HttpServer Server { get; private set; }

        public IDependencyResolver DependencyResolver { get; set; }
        public string Scheme { get; set; }
        public string Domain { get; set; }
        public int Port { get; set; }
        public Func<Uri, HttpServer> ServerFactory { get; set; }
        public Action<HttpFilterCollection> FilterConfigurer { get; set; }
        public Action<HttpRouteCollection> RouteConfigurer { get; set; }
        public Action<HttpConfiguration> ServerConfigurer { get; set; }

        public WebApiConfigurator()
        {
            Scheme = "http";
            Domain = "localhost";
            Port = 8080;
        }

        public WebApiConfigurator UseDependencyResolver(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;

            return this;
        }

        public WebApiConfigurator ConfigureFilters(Action<HttpFilterCollection> filters)
        {
            FilterConfigurer = filters;
            return this;
        }

        public WebApiConfigurator ConfigureRoutes(Action<HttpRouteCollection> route)
        {
            RouteConfigurer = route;

            return this;
        }

        public WebApiConfigurator ConfigureServer(Action<HttpConfiguration> config)
        {
            ServerConfigurer = config;

            return this;
        }

        public WebApiConfigurator OnLocalhost(int port = 8080)
        {
            return OnHost("http", "localhost", port);
        }

        public WebApiConfigurator OnHost(string scheme = null, string domain = null, int port = 8080)
        {
            Scheme = !string.IsNullOrEmpty(scheme) ? scheme : Scheme;
            Domain = !string.IsNullOrEmpty(domain) ? domain : Domain;
            Port = port;

            return this;
        }

        public HttpServer Build()
        {
            var log = HostLogger.Get(typeof(WebApiConfigurator));

            var baseAddress = new UriBuilder(Scheme, Domain, Port).Uri;

            log.Debug(string.Format("[Topshelf.WebApi] Configuring WebAPI Selfhost for URI: {0}", baseAddress));

            Server = ServerFactory != null ? ServerFactory(baseAddress) : BuildServer(baseAddress);
            var config = Server.Configuration;

            if (DependencyResolver != null)
                config.DependencyResolver = DependencyResolver;

            if (ServerConfigurer != null)
            {
                ServerConfigurer(config);
            }

            if (RouteConfigurer != null)
            {
                RouteConfigurer(config.Routes);
            }

            if (FilterConfigurer != null)
            {
                FilterConfigurer(config.Filters);
            }

            log.Info(string.Format("[Topshelf.WebApi] WebAPI Selfhost server configurated and listening on: {0}", baseAddress));

            return Server;
        }

        private HttpServer BuildServer(Uri baseAddress)
        {
            return new HttpSelfHostServer(new HttpSelfHostConfiguration(baseAddress));
        }

        public virtual void Initialize()
        {
            if (Server is HttpSelfHostServer)
                (Server as HttpSelfHostServer).OpenAsync().Wait();
        }

        public virtual void Shutdown()
        {
            if (Server is HttpSelfHostServer)
                (Server as HttpSelfHostServer).CloseAsync().Wait();
        }
    }
}