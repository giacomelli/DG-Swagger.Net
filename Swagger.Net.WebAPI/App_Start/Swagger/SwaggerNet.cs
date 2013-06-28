using System;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using Swagger.Net;
using Swagger.Net.ResourceModels.Configuration;
using Swagger.Net.WebApi.Controllers;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Swagger.Net.WebApi.App_Start.Swagger.SwaggerNet), "PreStart")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(Swagger.Net.WebApi.App_Start.Swagger.SwaggerNet), "PostStart")]
namespace Swagger.Net.WebApi.App_Start.Swagger
{
    public static class SwaggerNet
    {
        public static void PreStart()
        {
            RouteTable.Routes.MapHttpRoute(
                name: "SwaggerApi",
                routeTemplate: "api/docs/{controller}",
                defaults: new { swagger = true }
            );
        }

        public static void PostStart()
        {
            var config = GlobalConfiguration.Configuration;

            config.Filters.Add(new SwaggerActionFilter());

            try
            {
                config.Services.Replace(typeof(IDocumentationProvider),
                    new XmlCommentDocumentationProvider(HttpContext.Current.Server.MapPath("~/bin/Swagger.Net.WebApi.XML")));
            }
            catch (FileNotFoundException)
            {
                throw new Exception("Please enable \"XML documentation file\" in project properties with default (bin\\Swagger.Net.WebApi.XML) value or edit value in App_Start\\SwaggerNet.cs");
            }

            ResourcesConfiguration.Initialize(typeof(SwaggerNet).Assembly);
		}
    }
}