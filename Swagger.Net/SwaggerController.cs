﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Swagger.Net
{
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
			var docProvider = (XmlCommentDocumentationProvider)GlobalConfiguration.Configuration.Services.GetDocumentationProvider();

			ResourceListing r = SwaggerGen.CreateResourceListing(ControllerContext);
			List<string> uniqueControllers = new List<string>();

			foreach (var api in GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions)
			{
				if (api.ActionDescriptor.ControllerDescriptor.ControllerType.GetCustomAttributes(typeof(SwaggerIgnoreAttribute), false).Length == 0)
				{
					string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
					if (uniqueControllers.Contains(controllerName) ||
						  controllerName.ToUpper().Equals(SwaggerGen.SWAGGER.ToUpper())) continue;

					uniqueControllers.Add(controllerName);

					ResourceApi rApi = SwaggerGen.CreateResourceApi(api);
					r.apis.Add(rApi);

					// Model
					foreach (var param in api.ParameterDescriptions)
					{
						r.models.Add(SwaggerGen.CreateResourceModel(param, docProvider));
					}
				}
			}

			r.apis = r.apis.OrderBy(a => a.path).ToList();

			HttpResponseMessage resp = new HttpResponseMessage();

			resp.Content = new ObjectContent<ResourceListing>(r, ControllerContext.Configuration.Formatters.JsonFormatter);            
			
			return resp;
		}
	}
}
