using System.Web;

namespace Swagger.Net.ResourceModels.Configuration
{
    /// <summary>
    /// A runtime implementation of IConfigurationProxy.
    /// </summary>
    internal class RuntimeConfigurationProxy : IConfigurationProxy
    {
        /// <summary>
        /// Gets the header value.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        public string GetHeaderValue(string headerName)
        {
            return HttpContext.Current.Request.Headers[headerName];
        }
    }
}
