using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration.Operators
{
	/// <summary>
	/// Contains operator.
	/// </summary>
    internal class ContainsOperator : ILogicOperator
    {
		#region Fields
        private IOperand m_source;
        private IOperand m_contains;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Swagger.Net.ResourceModels.Configuration.Operators.ContainsOperator"/> class.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="contains">Contains.</param>
        public ContainsOperator(IOperand source, IOperand contains)
        {
            m_source = source;
            m_contains = contains;
        }

		/// <summary>
		/// Execute the operator with the specified state.
		/// </summary>
		/// <param name="state">State.</param>
        public bool Execute(object state)
        {
            var sourceValue = m_source.GetValue(state);
            var containsValue = m_contains.GetValue(state);

            if (sourceValue == null && containsValue == null)
            {
                return true;
            }

            if (sourceValue == null || containsValue == null)
            {
                return false;
            }

            return sourceValue.ToString().Contains(containsValue.ToString());
        }
		#endregion
    }
}
