using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration
{
	/// <summary>
	/// An operand base class.
	/// </summary>
    public abstract class OperandBase : IOperand
    {
		#region Properties
		/// <summary>
		/// Gets or sets the operator.
		/// </summary>
		/// <value>The operator.</value>
		public ILogicOperator Operator { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="state">State.</param>
        public abstract object GetValue(object state);
		#endregion        
    }
}
