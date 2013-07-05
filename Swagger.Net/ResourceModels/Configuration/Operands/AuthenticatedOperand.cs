using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Swagger.Net.ResourceModels.Configuration.Operands
{
    /// <summary>
    /// Authenticated operand.
    /// </summary>
    public class AuthenticatedOperand : OperandBase
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="state">State.</param>
        /// <returns>
        /// The value.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object GetValue(object state)
        {
            var user = HttpContext.Current.User;

            return user == null || user.Identity.IsAuthenticated;
        }
    }
}
