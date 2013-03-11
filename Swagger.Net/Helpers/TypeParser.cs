using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace Swagger.Net.Helpers
{
	/// <summary>
	/// A parser to Swagger data types as defined at https://github.com/wordnik/swagger-core/wiki/Datatypes.
	/// </summary>
	public static class TypeParser
	{
		#region Fields
		private static Dictionary<Type, string> s_typesMapping;
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes the <see cref="TypeParser"/> class.
		/// </summary>
		static TypeParser()
		{
			s_typesMapping = new Dictionary<Type, string>();
			s_typesMapping.Add(typeof(byte), "byte");
			s_typesMapping.Add(typeof(bool), "boolean");
			s_typesMapping.Add(typeof(int), "int");
			s_typesMapping.Add(typeof(long), "long");
			s_typesMapping.Add(typeof(float), "float");
			s_typesMapping.Add(typeof(double), "double");
			s_typesMapping.Add(typeof(string), "string");
			s_typesMapping.Add(typeof(DateTime), "Date");
			s_typesMapping.Add(typeof(char), "string");
			s_typesMapping.Add(typeof(short), "int");
		}
		#endregion

		#region Methods
		/// <summary>
		/// Parses the specified type to the Swagger data type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The equivalent Swagger date type.</returns>
		public static string Parse(Type type)
		{
			var result = type.Name;

			if (s_typesMapping.ContainsKey(type))
			{
				result = s_typesMapping[type];
			}
			else if (type.IsGenericType && type.GetInterface("IEnumerable") != null)
			{
				result = String.Format(CultureInfo.InvariantCulture, "List[{0}]", type.GetGenericArguments().First().Name);
			}

			return result;
		}

		/// <summary>
		/// Parses the specified type to the Swagger data type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The equivalent Swagger date type.</returns>
		public static string Parse(string type)
		{
			var result = type;

			if (type.Equals("IEnumerable"))
			{
				result = "List";
			}
			else if (type.StartsWith("IEnumerable<"))
			{
				result = type.Replace("IEnumerable<", "List[").Replace(">", "]");
			}
			else if (type.StartsWith("List<"))
			{
				result = type.Replace("List<", "List[").Replace(">", "]");
			}

			return result;
		}
		#endregion
	}
}
