using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Reflection;

namespace Swagger.Net.ResourceModels.Configuration
{
    /// <summary>
    /// Resposinble to load and initialize all ResourceConfiguration.
    /// </summary>
	public static class ResourcesConfiguration
	{
		#region Fields
		private static List<IResourceConfiguration> s_configurations = new List<IResourceConfiguration>();
		#endregion

		#region Methods
        /// <summary>
        /// Initializes all ResourceConfiguration from the specified assembly.
        /// </summary>
        /// <param name="configurationsAssembly">The configurations assembly.</param>
        public static void Initialize(Assembly configurationsAssembly)
        {
            var types = configurationsAssembly.GetTypes();
            var configs = types.Where(t => t.GetInterface("IResourceConfiguration") != null && !t.IsAbstract);

            foreach (var config in configs)
            {
                var instance = Activator.CreateInstance(config);
                s_configurations.Add((IResourceConfiguration)instance);
            }                       
        }        

        /// <summary>
        /// Determines whether the controller (ResourceConfiguration) can be shown.
        /// </summary>
        /// <param name="controllerDescriptor">The controller descriptor.</param>
        internal static bool IsResourceMapped(HttpControllerDescriptor controllerDescriptor)
		{
			var result = true;
			var config = GetResourceConfiguration (controllerDescriptor);

			if (config != null) {

                var canShow = config.CanShow;

                if (canShow.HasValue)
                {
                    result = canShow.Value;
                }
			}

			return result;
		}

        /// <summary>
        /// Determines whether the ApiDescription (OperationConfiguration) can be shown.
        /// </summary>
        /// <param name="apiDescription">The API description.</param>  
		internal static bool IsOperationMapped(ApiDescription apiDescription)
		{
			var result = true;
            var actionDescriptor = apiDescription.ActionDescriptor;
            var config = GetResourceConfiguration(actionDescriptor.ControllerDescriptor);

			if (config != null) {
                result = config.IsOperationMapped(apiDescription.HttpMethod, apiDescription.RelativePath);
			}

			return result;
		}


        /// <summary>
        /// Determines whether the error response (ErrorResponseConfiguration) can be shown.
        /// </summary>
        /// <param name="errorResponse">The error response.</param>
        internal static bool IsErrorMessageMapped(ApiDescription apiDescription, ResourceApiOperationParameterErrorResponse errorResponse)
        {
            var result = true;
            var operation = GetOperationConfiguration(apiDescription, o => o.HasErrorResponses);

            if (operation != null)
            {
                result = operation.CanShowErrorResponse(errorResponse);
            }

            return result;
        }
		#endregion        

        #region Helpers
        /// <summary>
        /// Gets the controller configuration.
        /// </summary>
        /// <param name="controllerDescriptor">The controller descriptor.</param>
        /// <returns></returns>
        private static IResourceConfiguration GetResourceConfiguration(HttpControllerDescriptor controllerDescriptor)
        {
            var controllerType = controllerDescriptor.ControllerType;
            return s_configurations.FirstOrDefault(c => c.ControllerType.Equals(controllerType));
        }

        private static OperationConfiguration GetOperationConfiguration(ApiDescription apiDescription, Func<OperationConfiguration, bool> where = null)
        {
            OperationConfiguration result = null;

            var config = GetResourceConfiguration(apiDescription.ActionDescriptor.ControllerDescriptor);

            if (config != null)
            {
                if (where == null)
                {
                    result = config.GetOperation(apiDescription.HttpMethod, apiDescription.RelativePath);
                }
                else
                {
                    result = config.GetOperation(apiDescription.HttpMethod, apiDescription.RelativePath, where); 
                }
            }

            return result;
        }
        #endregion
    }
}