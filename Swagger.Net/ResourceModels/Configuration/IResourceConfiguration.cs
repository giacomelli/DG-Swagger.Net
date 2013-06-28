using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace Swagger.Net.ResourceModels.Configuration
{
    /// <summary>
    /// Defines a inteface for a resource configuration.
    /// </summary>
	public interface IResourceConfiguration
	{
        /// <summary>
        /// Gets the type of the controller.
        /// </summary>
		Type ControllerType { get; }

        /// <summary>
        /// Gets if the targer of configuration can be shown.
        /// </summary>
		bool? CanShow { get; }

        /// <summary>
        /// Determines whether this the operation can be shown.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="path">The path.</param>
    	bool IsOperationMapped(HttpMethod httpMethod, string path);


        OperationConfiguration GetOperation(HttpMethod httpMethod, string path);
        OperationConfiguration GetOperation(HttpMethod httpMethod, string path, Func<OperationConfiguration, bool> where);
	}
}

