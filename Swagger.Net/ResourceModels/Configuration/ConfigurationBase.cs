using System;
using System.Collections.Generic;
using System.Linq;

namespace Swagger.Net.ResourceModels.Configuration
{
    /// <summary>
    /// An configuration base class.
    /// </summary>
    public abstract class ConfigurationBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationBase"/> class.
        /// </summary>        
        protected ConfigurationBase()
        {            
            LeftOperands = new List<IOperand>();
        }
        #endregion

        #region Properties  
		/// <summary>
		/// Gets the left operands.
		/// </summary>
		/// <value>The left operands.</value>
        protected IList<IOperand> LeftOperands { get; private set; }
        #endregion

        #region Methods
		/// <summary>
		/// Determines whether this instance is mapped the specified state.
		/// </summary>
		/// <returns><c>true</c> if this instance is mapped the specified state; otherwise, <c>false</c>.</returns>
		/// <param name="state">State.</param>
        internal bool IsMapped(object state)
        {
            if (LeftOperands.Count > 0)
            {
                return LeftOperands.Any(op => op.Operator.Execute(state));
            }

            return true;
        }
        #endregion
    }
}
