using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration
{
	/// <summary>
	/// Defines an interface for a logic operator.
	/// </summary>
    public interface ILogicOperator
    {
		/// <summary>
		/// Execute the operator with the specified state.
		/// </summary>
		/// <param name="state">State.</param>
        bool Execute(object state);
    }
}
