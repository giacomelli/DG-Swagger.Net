using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration.Operators
{
	/// <summary>
	/// Negation operator.
	/// </summary>
    public class NegationOperator : ILogicOperator
    {
		#region Fields
        private ILogicOperator m_underlyingOperator;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Swagger.Net.ResourceModels.Configuration.Operators.NegationOperator"/> class.
		/// </summary>
		/// <param name="underlyingOperator">Underlying operator.</param>
        public NegationOperator(ILogicOperator underlyingOperator)
        {
            m_underlyingOperator = underlyingOperator;
        }
		#endregion

		#region Methods
		/// <summary>
		/// Execute the operator with the specified state.
		/// </summary>
		/// <param name="state">State.</param>
        public bool Execute(object state)
        {
            return !m_underlyingOperator.Execute(state);
        }
		#endregion
    }
}
