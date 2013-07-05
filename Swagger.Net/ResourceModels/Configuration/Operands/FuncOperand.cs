using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration.Operands
{
    /// <summary>
    /// A func operand.
    /// </summary>
    public class FuncOperand : OperandBase
    {
        #region Fields
        private Func<object, object> m_func;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FuncOperand"/> class.
        /// </summary>
        /// <param name="func">The func.</param>
        public FuncOperand(Func<object, object> func)
        {
            m_func = func;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="state">State.</param>
        /// <returns>
        /// The value.
        /// </returns>
        public override object GetValue(object state)
        {
            return m_func(state);
        }
        #endregion
    }
}
