using System;

namespace Swagger.Net
{
	/// <summary>
	/// Attribute to configure some options on Swagger.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	public class SwaggerOptionsAttribute : Attribute
	{
		#region Properties
		/// <summary>
		/// Gets or sets the name of target on Swagger.
		/// </summary>
		public string Name { get; set; }
		#endregion
	}
}
