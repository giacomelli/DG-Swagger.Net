using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration.Operands
{
	/// <summary>
	/// Fixed value operand.
	/// </summary>
    public class FixedValueOperand : OperandBase
    {
		#region Fields
        private object m_value;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Swagger.Net.ResourceModels.Configuration.Operands.FixedValueOperand"/> class.
		/// </summary>
		/// <param name="value">Value.</param>
        public FixedValueOperand(object value)
        {
            m_value = value;
        }

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="state">State.</param>
        public override object GetValue(object state)
        {
            return m_value;
        }
		#endregion
    }
}
