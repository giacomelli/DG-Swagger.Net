using System;
using System.Diagnostics;
using Swagger.Net.ResourceModels.Configuration.Operands;

namespace Swagger.Net.ResourceModels.Configuration
{
    /// <summary>
    /// Represents a configuration for operation's error responses.
    /// </summary>    
    public class ErrorResponsesConfiguration : ConfigurationBase
    {           
        /// <summary>
        /// Map an atrribute write on XML documentation of tag "Exception".
        /// </summary>
        /// <param name="attributeName">The attribute name</param>
        public OperandBase Attribute(string attributeName)
        {
            var operand = new ErrorResponseAttributeOperand(attributeName);            
            LeftOperands.Add(operand);

            return operand;
        }
    }
}