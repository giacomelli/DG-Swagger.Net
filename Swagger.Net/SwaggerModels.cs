using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Dynamic;
using Newtonsoft.Json;
using Swagger.Net.ResourceModels;
using Swagger.Net.Helpers;
using System.Configuration;
using System.Diagnostics;
using DocsByReflection;

namespace Swagger.Net
{
	public static class SwaggerGen
	{
		#region Constants
		public const string SWAGGER = "swagger";
		public const string SWAGGER_VERSION = "2.0";
		public const string FROMURI = "FromUri";
		public const string FROMBODY = "FromBody";
		public const string QUERY = "query";
		public const string PATH = "path";
		public const string BODY = "body";
		#endregion

		#region Fields
		private static Assembly s_targetAssembly;
		private static string s_apiVersion;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes the <see cref="SwaggerGen"/> class.
		/// </summary>
		static SwaggerGen()
		{
			var apiTargetAssemblyName = ConfigurationManager.AppSettings["Swagger:ApiTargetAssemblyName"];
			s_targetAssembly = String.IsNullOrEmpty(apiTargetAssemblyName) ? Assembly.GetCallingAssembly() : Assembly.Load(apiTargetAssemblyName);
			s_apiVersion = s_targetAssembly.GetName().Version.ToString();
		}
		#endregion

		/// <summary>
		/// Create a resource listing
		/// </summary>
		/// <param name="actionContext">Current action context</param>
		/// <param name="includeResourcePath">Should the resource path property be included in the response</param>
		/// <returns>A resource Listing</returns>
		public static ResourceListing CreateResourceListing(HttpActionContext actionContext, bool includeResourcePath = true)
		{
			return CreateResourceListing(actionContext.ControllerContext, includeResourcePath);
		}

		/// <summary>
		/// Create a resource listing
		/// </summary>
		/// <param name="actionContext">Current controller context</param>
		/// <param name="includeResourcePath">Should the resource path property be included in the response</param>
		/// <returns>A resrouce listing</returns>
		public static ResourceListing CreateResourceListing(HttpControllerContext controllerContext, bool includeResourcePath = false)
		{
			var uri = controllerContext.Request.RequestUri;
			
			ResourceListing rl = new ResourceListing()
			{
								
				ApiVersion =  s_apiVersion,
				SwaggerVersion = SWAGGER_VERSION,
				BasePath = uri.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/'),				
				Models = new ResourceModelNodeCollection()
			};

			if (includeResourcePath)
			{
				rl.ResourcePath = controllerContext.ControllerDescriptor.ControllerName; 
			}

			return rl;
		}

		/// <summary>
		/// Create an api element 
		/// </summary>
		/// <param name="api">Description of the api via the ApiExplorer</param>
		/// <returns>A resource api</returns>
		public static ResourceApi CreateResourceApi(ApiDescription api)
		{			
			ResourceApi rApi = new ResourceApi()
			{
				path = "/" + api.RelativePath,
                description = DocsService.GetXmlFromType(api.ActionDescriptor.ControllerDescriptor.ControllerType, false).InnerText,
				operations = new List<ResourceApiOperation>()
			};

			CustomAttributeHelper.PrepareByOptionAttribute(rApi, api.ActionDescriptor);

			return rApi;
		}

        private static Dictionary<Type, IList<ResourceModelNode>> s_cache = new Dictionary<Type, IList<ResourceModelNode>>();

		private static object s_lock = new object();

		public static IList<ResourceModelNode> CreateResourceModel(ApiParameterDescription param, XmlCommentDocumentationProvider docProvider)
		{
			return CreateResourceModel(param, param.ParameterDescriptor.ParameterType, docProvider);
		}

        public static IList<ResourceModelNode> CreateResourceModel(Type modelType, XmlCommentDocumentationProvider docProvider)
		{
            return CreateResourceModel(null, modelType, docProvider);
		}

        private static IList<ResourceModelNode> CreateResourceModel(ApiParameterDescription param, Type modelType, XmlCommentDocumentationProvider docProvider)
		{
			lock (s_lock)
			{				
                var result = new List<ResourceModelNode>();
				ResourceModelNode rModel = null;
			
				if ((!modelType.IsEnum && !modelType.Equals(typeof(string))))
				{
					if (modelType.IsGenericType)
					{
						modelType = modelType.GetGenericArguments().First();
						return CreateResourceModel(param, modelType, docProvider);
					}
                    
					if (s_cache.ContainsKey(modelType))
					{
                        result.AddRange(s_cache[modelType]);
                        return result;
					}


					rModel = new ResourceModelNode()
					{
                        Id = TypeParser.Parse(modelType.Name)
					};

                    s_cache.Add(modelType, result);

					foreach (var typeProperty in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
					{
						var property = new ResourceModelPropertyNode();
						property.Id = TypeParser.Parse(typeProperty.Name);
						property.Type = TypeParser.Parse(typeProperty.PropertyType);

                        if (typeProperty.PropertyType.IsValueType || Nullable.GetUnderlyingType(typeProperty.PropertyType) == null)
                        {
                            rModel.Required.Add(property.Id);
                        }

                        if (property.Type.StartsWith("List["))
                        {
                            property.Type = "array";
                            property.DefineContainerType(typeProperty.PropertyType.GetGenericArguments().First());
                        }

                        result.AddRange(CreateResourceModel(typeProperty.PropertyType, docProvider));                        

						if (docProvider != null)
						{
                            property.Description = docProvider.GetDocumentation(typeProperty);

                            if (typeProperty.PropertyType.IsEnum)
                            {
                                property.AllowableValues = CreateAllowableValues(typeProperty.PropertyType);
                               
                            }
                            else if((typeProperty.PropertyType.IsClass || typeProperty.PropertyType.IsValueType) && typeof(string) != typeProperty.PropertyType)
                            {
                                result.AddRange(CreateResourceModel(typeProperty.PropertyType, docProvider));

                            }
						}

						rModel.Properties.Add(property);
					}

                    if (rModel.Properties.Count > 0)
                    {
                        result.Add(rModel);
                    }
				}

				return result;
			}
		}

		private static ResourceModelPropertyAllowableValuesNode CreateAllowableValues(Type type)
		{
			ResourceModelPropertyAllowableValuesNode allowableValues = null;

			if (type.IsEnum)
			{
				var enumValues = Enum.GetNames(type);
				allowableValues = new ResourceModelPropertyAllowableValuesNode();

				foreach (var v in enumValues)
				{
					allowableValues.Values.Add(v);
				}
			}

			return allowableValues;
		}

		/// <summary>
		/// Creates an api operation
		/// </summary>
		/// <param name="api">Description of the api via the ApiExplorer</param>
		/// <param name="docProvider">Access to the XML docs written in code</param>
		/// <returns>An api operation</returns>
		public static ResourceApiOperation CreateResourceApiOperation(ApiDescription api, XmlCommentDocumentationProvider docProvider)
		{
			ResourceApiOperation rApiOperation = new ResourceApiOperation()
			{
				httpMethod = api.HttpMethod.ToString(),
				nickname = docProvider.GetNickname(api.ActionDescriptor),
				responseClass = TypeParser.Parse(docProvider.GetResponseClass(api.ActionDescriptor)),
				summary = api.Documentation,
				notes = docProvider.GetNotes(api.ActionDescriptor),
				parameters = new List<ResourceApiOperationParameter>(),
				errorResponses = docProvider.GetErrorResponses(api)
			};

			return rApiOperation;
		}

		/// <summary>
		/// Creates an operation parameter
		/// </summary>
		/// <param name="api">Description of the api via the ApiExplorer</param>
		/// <param name="param">Description of a parameter on an operation via the ApiExplorer</param>
		/// <param name="docProvider">Access to the XML docs written in code</param>
		/// <returns>An operation parameter</returns>
		public static ResourceApiOperationParameter CreateResourceApiOperationParameter(ApiDescription api, ApiParameterDescription param, XmlCommentDocumentationProvider docProvider)
		{
			string paramType = (param.Source.ToString().Equals(FROMURI)) ? QUERY : BODY;

			var parameter = new ResourceApiOperationParameter()
			{
				paramType = (paramType == "query" && api.RelativePath.IndexOf("{" + param.Name + "}") > -1) ? PATH : paramType,
				name = param.Name,
				description = param.Documentation,
				dataType = TypeParser.Parse(param.ParameterDescriptor.ParameterType),
				required = docProvider.GetRequired(param.ParameterDescriptor)				
			};

			parameter.allowMultiple = parameter.dataType.StartsWith("List[");
			parameter.allowableValues = CreateAllowableValues(param.ParameterDescriptor.ParameterType);

			CustomAttributeHelper.PrepareByOptionAttribute(parameter, param.ParameterDescriptor);

			return parameter;
		}		
	}	

	public class ResourceApi
	{
		public string path { get; set; }
		public string description { get; set; }
		public List<ResourceApiOperation> operations { get; set; }
	}	

	public class ResourceApiOperation
	{
		public string httpMethod { get; set; }
		public string nickname { get; set; }
		public string responseClass { get; set; }
		public string summary { get; set; }
		public string notes { get; set; }
		public List<ResourceApiOperationParameter> parameters { get; set; }
		public IList<ResourceApiOperationParameterErrorResponse> errorResponses { get; set; }
	}

	public class ResourceApiOperationParameter
	{
		public ResourceApiOperationParameter()
		{
		}

		public string paramType { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string dataType { get; set; }
		public bool required { get; set; }
		public bool allowMultiple { get; set; }
		public ResourceModelPropertyAllowableValuesNode allowableValues { get; set; }		
	}

	/// <summary>
	/// Represents a Swagger's errorResponse as defined at https://github.com/wordnik/swagger-core/wiki/Errors.
	/// <remarks>
	/// To use it, specify the exceptions on you api action controller:
	/// <example>
	/// &lt;exception code="400" message="Agency name should be unique." /&gt;
	/// &lt;exception code="400" message="You must specify the agency." /&gt;
	/// </example>
	/// </remarks>
	/// </summary>
    [DebuggerDisplay("{Code}: {Reason} ({ExtraAttributes.Count})")]
    public class ResourceApiOperationParameterErrorResponse
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceApiOperationParameterErrorResponse"/> class.
        /// </summary>
        public ResourceApiOperationParameterErrorResponse()
        {
            ExtraAttributes = new Dictionary<string, string>();
        }
        #endregion

        #region Properties
        /// <summary>
		/// Gets or sets the HTTP Status code.
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Gets os sets the message reason.
		/// </summary>
		public string Reason { get; set; }

        /// <summary>
        /// Gets the extra attributes.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, string> ExtraAttributes { get; private set; }
		#endregion
	}
}