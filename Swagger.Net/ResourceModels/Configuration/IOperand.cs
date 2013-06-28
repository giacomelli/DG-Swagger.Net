using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration
{
	/// <summary>
	/// Defines an interface for an operand.
	/// </summary>
    public interface IOperand
    {
		#region Properties
		/// <summary>
		/// Gets or sets the operator.
		/// </summary>
		/// <value>The operator.</value>
		ILogicOperator Operator { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="state">State.</param>
        object GetValue(object state);
		#endregion        
    }
}
