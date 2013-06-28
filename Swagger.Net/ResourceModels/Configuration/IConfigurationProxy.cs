
namespace Swagger.Net.ResourceModels.Configuration
{
    /// <summary>
    /// Defines an interface for a configuration proxy.
    /// </summary>
    public interface IConfigurationProxy
    {
        /// <summary>
        /// Gets the header value.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <returns></returns>
        string GetHeaderValue(string headerName);
    }
}
