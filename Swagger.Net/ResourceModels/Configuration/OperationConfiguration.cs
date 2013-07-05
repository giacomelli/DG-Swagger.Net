using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Linq;
using Swagger.Net.ResourceModels.Configuration.Operands;
using System;
using Swagger.Net.ResourceModels.Configuration.Operators;

namespace Swagger.Net.ResourceModels.Configuration
{
    /// <summary>
    /// Represents a configuration for an API operation.
    /// </summary>
    [DebuggerDisplay("{HttpMethod} {Path} {CanShow}")]
    public class OperationConfiguration : ConfigurationBase
    {
        #region Fields
        private ErrorResponsesConfiguration m_errorResponses;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationConfiguration"/> class.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="path">The path.</param>
        /// <param name="proxy">The proxy.</param>
        internal OperationConfiguration(HttpMethod httpMethod, string path)            
        {
            HttpMethod = httpMethod;
            Path = path;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public HttpMethod HttpMethod { get; private set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; private set; }

        internal bool HasErrorResponses
        {
            get
            {
                return m_errorResponses != null;
            }
        }
        public ErrorResponsesConfiguration ErrorResponses
        {
            get
            {
                if (m_errorResponses == null)
                {
                    m_errorResponses = new ErrorResponsesConfiguration();
                }

                return m_errorResponses;
            }
        }
        #endregion

        #region Methods        

        public OperandBase HttpHeader(string headerName)
        {
            var operand = new HttpHeaderOperand(headerName);
            LeftOperands.Add(operand);

            return operand;
        }

        public OperandBase Authenticated()
        {
            var operand = new AuthenticatedOperand();
            LeftOperands.Add(operand);

            return operand;
        }

        public OperandBase Func(Func<object, object> func)
        {
            var operand = new FuncOperand(func);
            LeftOperands.Add(operand);

            return operand;
        }

        public void IsTrue(Func<object, bool> func)
        {
            var operand = new FuncOperand((o) =>
            {
                return func(o);
            });

            operand.Operator = new EqualsOperator(operand, new FixedValueOperand(true));

            LeftOperands.Add(operand);
        }
     
        internal bool CanShowErrorResponse(ResourceApiOperationParameterErrorResponse errorResponse)
        {
            if (m_errorResponses == null)
            {
                return true;
            }

            return m_errorResponses.IsMapped(errorResponse);
        }
        #endregion        
    }
}