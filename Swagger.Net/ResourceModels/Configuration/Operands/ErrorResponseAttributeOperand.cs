using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration.Operands
{
	/// <summary>
	/// ErrorRsesponse attribute operand.
	/// </summary>
    public class ErrorResponseAttributeOperand : OperandBase
    {
        #region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Swagger.Net.ResourceModels.Configuration.Operands.ErrorResponseAttributeOperand"/> class.
		/// </summary>
		/// <param name="attributeName">Attribute name.</param>
        public ErrorResponseAttributeOperand(string attributeName)        
        {
            AttributeName = attributeName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        /// <value>
        /// The name of the header.
        /// </value>
        public string AttributeName { get; private set; }

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="state">State.</param>
        public override object GetValue(object state)
        {
            object result = null;
            var errorResponse = state as ResourceApiOperationParameterErrorResponse;

            if (errorResponse != null && errorResponse.ExtraAttributes.ContainsKey(AttributeName))
            {
                result = errorResponse.ExtraAttributes[AttributeName];
            }

            return result;
        }
        #endregion   
    }
}
