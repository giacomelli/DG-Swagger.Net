using System.Collections.Generic;

namespace Swagger.Net.ResourceModels
{	
	/// <summary>
	/// A Swagger model as defined at https://github.com/wordnik/swagger-core/wiki/Datatypes (Complex Types).
	/// </summary>
	public class ResourceModelNode : ResourceModelNodeBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new <see cref="ResourceModelNode"/> class instance.
		/// </summary>
		public ResourceModelNode()
		{
			Properties = new ResourceModelPropertyNodeCollection();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the properties.
		/// </summary>
		public ResourceModelPropertyNodeCollection Properties { get; private set;}
		#endregion
	}
}
