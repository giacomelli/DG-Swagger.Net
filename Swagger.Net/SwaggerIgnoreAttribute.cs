using System;

namespace Swagger.Net
{
	/// <summary>
	/// Attribute to mark a Controller to be ignored.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class SwaggerIgnoreAttribute : Attribute
	{
	}
}
