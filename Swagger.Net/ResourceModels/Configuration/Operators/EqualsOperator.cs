using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration.Operators
{
	/// <summary>
	/// Equals operator.
	/// </summary>
    public class EqualsOperator : ILogicOperator
    {
		#region Fields
        private IOperand[] m_operands;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Swagger.Net.ResourceModels.Configuration.Operators.EqualsOperator"/> class.
		/// </summary>
		/// <param name="operands">Operands.</param>
        public EqualsOperator(params IOperand[] operands)
        {
            if (operands.Length < 2)
            {
                throw new InvalidOperationException("An EqualsOperator need at least 2 operands.");
            }

            m_operands = operands;
        }
		#endregion

		#region Methods
		/// <summary>
		/// Execute the operator with the specified state.
		/// </summary>
		/// <param name="state">State.</param>
        public virtual bool Execute(object state)
        {
            if(m_operands.All(o => o.GetValue(state) == null))
            {
                return true;
            }

            var firstValue = m_operands[0].GetValue(state);

            if (firstValue == null)
            {
                return false;
            }

            return m_operands.Skip(1).Any(o => firstValue.Equals(o.GetValue(state)));
        }
		#endregion
    }
}
