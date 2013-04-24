namespace Swagger.Net.ResourceModels
{
	/// <summary>
	/// Represents a ResourceModel's property.
	/// </summary>
	public class ResourceModelPropertyNode : ResourceModelNodeBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new <see cref="ResourceModelPropertyNode"/> instance.
		/// </summary>
		public ResourceModelPropertyNode()
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the allowable values.
		/// </summary>
		public ResourceModelPropertyAllowableValuesNode AllowableValues { get; set; }
		#endregion
	}
}
