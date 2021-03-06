﻿using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Swagger.Net.Helpers;
using Swagger.Net.ResourceModels.Configuration;
using WebApiProxy;

namespace Swagger.Net
{
    /// <summary>
    /// Swagger controller.
    /// </summary>
    [ExcludeProxy]
    public class SwaggerController : ApiController
    {
        /// <summary>
        /// Get the resource description of the api for swagger documentation
        /// </summary>
        /// <remarks>It is very convenient to have this information available for generating clients. This is the entry point for the swagger UI
        /// </remarks>
        /// <returns>JSON document representing structure of API</returns>
        public HttpResponseMessage Get()
        {
            //var formatter = RequestContext.Configuration.Formatters.JsonFormatter;
            //formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var docProvider = (XmlCommentDocumentationProvider)GlobalConfiguration.Configuration.Services.GetDocumentationProvider();

            ResourceListing r = SwaggerGen.CreateResourceListing(ControllerContext);
            List<string> uniqueControllers = new List<string>();

            foreach (var api in GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions)
            {
                if (!CustomAttributeHelper.HasIgnoreAttribute(api.ActionDescriptor.ControllerDescriptor)
                    && ResourcesConfiguration.IsResourceMapped(api.ActionDescriptor.ControllerDescriptor))
                {
                    string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
                    if (uniqueControllers.Contains(controllerName) ||
                          controllerName.ToUpper().Equals(SwaggerGen.SWAGGER.ToUpper())) continue;

                    uniqueControllers.Add(controllerName);

                    ResourceApi rApi = SwaggerGen.CreateResourceApi(api);
                    var queryIndex = rApi.path.IndexOf("?");

                    if (queryIndex > -1)
                    {
                        rApi.path = rApi.path.Substring(0, queryIndex);
                    }

                    r.AddApi(rApi);

                    // Model
                    foreach (var param in api.ParameterDescriptions)
                    {
                        r.Models.AddRange(SwaggerGen.CreateResourceModel(param, docProvider));
                    }
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new ObjectContent<ResourceListing>(r, ControllerContext.Configuration.Formatters.JsonFormatter);

            return resp;
        }
    }
}
