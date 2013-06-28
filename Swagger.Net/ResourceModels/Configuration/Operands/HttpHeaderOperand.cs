using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration.Operands
{
	/// <summary>
	/// Http header operand.
	/// </summary>
    public class HttpHeaderOperand : OperandBase
    {
        #region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Swagger.Net.ResourceModels.Configuration.Operands.HttpHeaderOperand"/> class.
		/// </summary>
		/// <param name="headerName">Header name.</param>
        public HttpHeaderOperand(string headerName)        
        {
            HeaderName = headerName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the header.
        /// </summary>
        /// <value>
        /// The name of the header.
        /// </value>
        public string HeaderName { get; private set; }
		#endregion	

		#region Methods
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="state">State.</param>
        public override object GetValue(object state)
        {
            return ConfigurationProvider.Proxy.GetHeaderValue(HeaderName);            
        }
        #endregion       
    }
}
