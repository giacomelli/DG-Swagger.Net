using System;

namespace Swagger.Net
{
	/// <summary>
	/// Attribute to mark a Controller to be ignored.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class SwaggerIgnoreAttribute : Attribute
	{
	}
}
