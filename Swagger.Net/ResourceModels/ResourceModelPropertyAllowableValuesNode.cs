using System.Collections.Generic;
namespace Swagger.Net.ResourceModels
{
	#region
	/// <summary>
	/// Types of allowable value.
	/// </summary>
	public enum AllowableValueType 
	{
		/// <summary>
		/// A list of values.
		/// </summary>
		List
	}
	#endregion

	/// <summary>
	/// Represents a property allowable values node.
	/// </summary>
	public class ResourceModelPropertyAllowableValuesNode
	{
		#region Constructors
		/// <summary>
		/// Initializes a new <see cref="ResourceModelPropertyAllowableValuesNode"/> instance.
		/// </summary>
		public ResourceModelPropertyAllowableValuesNode()
		{
			Values = new List<string>();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the value type.
		/// </summary>
		public AllowableValueType ValueType { get; set; }

		/// <summary>
		/// Gets the values.
		/// </summary>
		public IList<string> Values { get; private set; }
		#endregion
	}
}
