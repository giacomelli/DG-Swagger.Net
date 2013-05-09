using System.Web.Http.Controllers;
using System;
using System.Web.Http.Description;
using System.Globalization;
namespace Swagger.Net.Helpers
{	
	/// <summary>
	/// Custom attribute helper.
	/// </summary>
	internal class CustomAttributeHelper
	{
		public static bool HasIgnoreAttribute(HttpActionDescriptor actionDescriptor)
		{
			return actionDescriptor.GetCustomAttributes<SwaggerIgnoreAttribute>().Count > 0;
		}

		public static bool HasIgnoreAttribute(HttpControllerDescriptor controllerDescriptor)
		{
			return controllerDescriptor.GetCustomAttributes<SwaggerIgnoreAttribute>().Count > 0;
		}

		public static void PrepareByOptionAttribute(ResourceApiOperationParameter parameter, HttpParameterDescriptor parameterDescriptor)
		{
			var attributes = parameterDescriptor.GetCustomAttributes<SwaggerOptionsAttribute>();

			if (attributes.Count > 0)
			{
				var att = attributes[0] as SwaggerOptionsAttribute;

				if (!String.IsNullOrWhiteSpace(att.Name))
				{
					parameter.name = att.Name;
				}
			}
		}

		public static void PrepareByOptionAttribute(ResourceApi resource, HttpActionDescriptor actionDescriptor)
		{
			var parameters = actionDescriptor.GetParameters();

			foreach (var p in parameters)
			{
				var attributes = p.GetCustomAttributes<SwaggerOptionsAttribute>();

				if (attributes.Count > 0)
				{
					var att = attributes[0] as SwaggerOptionsAttribute;

					if (!String.IsNullOrWhiteSpace(att.Name))
					{
						resource.path = resource.path.Replace(
							String.Format(CultureInfo.InvariantCulture, "{{{0}}}", p.ParameterName), 
							String.Format(CultureInfo.InvariantCulture, "{{{0}}}", att.Name));
					}
				}
			}
		}	
	}
}
