using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net.ResourceModels.Configuration
{
	/// <summary>
	/// Configuration provider.
	/// </summary>
    internal static class ConfigurationProvider
    {
		#region Constructors
		/// <summary>
		/// Initializes the <see cref="Swagger.Net.ResourceModels.Configuration.ConfigurationProvider"/> class.
		/// </summary>
        static ConfigurationProvider()
        {
            Proxy = new RuntimeConfigurationProxy();
        }
		#endregion

		#region Propreties
		/// <summary>
		/// Gets or sets the proxy.
		/// </summary>
		/// <value>The proxy.</value>
        public static IConfigurationProxy Proxy { get; internal set; }
		#endregion
    }
}
