using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Swagger.Net.Serialization;
using System.Collections.ObjectModel;
using System.Web.Http.Description;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Swagger.Net.Helpers;
using Swagger.Net.ResourceModels.Configuration;

namespace Swagger.Net
{
	/// <summary>
	/// Determines if any request hit the Swagger route. Moves on if not, otherwise responds with JSON Swagger spec doc
	/// </summary>
	public class SwaggerActionFilter : ActionFilterAttribute
	{
		#region Fields
		private XmlCommentDocumentationProvider m_docProvider;
		private IList<ApiDescription> m_apiDescriptions;
		#endregion

		#region Properties
		private XmlCommentDocumentationProvider DocProvider
		{
			get
			{
				if (m_docProvider == null)
				{
					m_docProvider = (XmlCommentDocumentationProvider)GlobalConfiguration.Configuration.Services.GetDocumentationProvider();
				}

				return m_docProvider;
			}
		}
		#endregion

		#region Methods
		private void CollectApiDescriptions()
		{
			if (m_apiDescriptions == null)
			{
				var query = from a in GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions
							where !a.Route.Defaults.ContainsKey(SwaggerGen.SWAGGER)
							select a;

				m_apiDescriptions = query.ToList();
			}
		}

		private IEnumerable<ApiDescription> GetApiDescriptionsByController(string controllerName)
		{
			return m_apiDescriptions.Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase));
		}
		#endregion

		/// <summary>
		/// Executes each request to give either a JSON Swagger spec doc or passes through the request
		/// </summary>
		/// <param name="actionContext">Context of the action</param>
        
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var docRequest = actionContext.ControllerContext.RouteData.Values.ContainsKey(SwaggerGen.SWAGGER);

			if (!docRequest)
			{
				base.OnActionExecuting(actionContext);
				return;
			}

			var response = new HttpResponseMessage();

			var formatter = actionContext.ControllerContext.Configuration.Formatters.JsonFormatter;
            
			var resourceListing = GetDocs(actionContext);

			formatter.SerializerSettings.ContractResolver = new SwaggerContractResolver(resourceListing);

			response.Content = new ObjectContent<ResourceListing>(
				resourceListing,
				formatter);

			actionContext.Response = response;
		}
               
		private ResourceListing GetDocs(HttpActionContext actionContext)
		{
			CollectApiDescriptions();
			var resourceListing = SwaggerGen.CreateResourceListing(actionContext);			
			var apis = GetApiDescriptionsByController(actionContext.ControllerContext.ControllerDescriptor.ControllerName);

            foreach (var api in apis)
            {
                if (ResourcesConfiguration.IsOperationMapped(api))
                {
                    var resourceApi = SwaggerGen.CreateResourceApi(api);
                    resourceListing.AddApi(resourceApi);

                
                    ResourceApiOperation resourceApiOperation = null;
                
                    if (!CustomAttributeHelper.HasIgnoreAttribute(api.ActionDescriptor))
                    {
                        resourceApiOperation = SwaggerGen.CreateResourceApiOperation(api, DocProvider);
                        resourceApi.operations.Add(resourceApiOperation);
                    }
                  
                    var reflectedActionDescriptor = api.ActionDescriptor as ReflectedHttpActionDescriptor;
                    resourceListing.Models.AddRange(SwaggerGen.CreateResourceModel(reflectedActionDescriptor.MethodInfo.ReturnType, DocProvider));
                  

                    foreach (var param in api.ParameterDescriptions)
                    {
                        ResourceApiOperationParameter parameter = SwaggerGen.CreateResourceApiOperationParameter(api, param, DocProvider);
                        resourceApiOperation.parameters.Add(parameter);
                        resourceListing.Models.AddRange(SwaggerGen.CreateResourceModel(param, DocProvider));
                    }
                }
            }

			return resourceListing;
		}
	}
}
