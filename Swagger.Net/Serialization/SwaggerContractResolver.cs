using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Swagger.Net.Serialization
{
	/// <summary>
	/// A contract resolver to deal with Swagger spec.
	/// </summary>
	public class SwaggerContractResolver : DefaultContractResolver
	{
		#region Fields
		private ResourceListing m_resourceListing;
		#endregion

		#region Constructors	
		/// <summary>
		/// Initializes a new <see cref="SwaggerContractResolver"/> class instance.
		/// <param name="resourceListing">The API resource listing.</param>
		/// </summary>
		public SwaggerContractResolver(ResourceListing resourceListing)
		{
			m_resourceListing = resourceListing;
		}

		/// <summary>
		/// Resolves the name of the property.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>
		/// Name of the property.
		/// </returns>
		protected override string ResolvePropertyName(string propertyName)
		{
			if (m_resourceListing.models.GetDynamicMemberNames().Contains(propertyName))
			{
				return propertyName;
			}

			return ToCamelCase(propertyName);
		}

		/// <summary>
		/// Transform the string to camel case.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>The source string in camel case.</returns>
		private static string ToCamelCase(string source)
		{
			var parts = source.Split(' ');
			var newString = new StringBuilder();

			foreach (var p in parts)
			{
				newString.AppendFormat("{0}{1}", Char.ToLowerInvariant(p[0]), p.Substring(1, p.Length - 1));
			}

			return newString.ToString();
		}
		#endregion
	}
}
