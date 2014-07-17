using System.Collections.Generic;
using System.Linq;
using Swagger.Net.ResourceModels;

namespace Swagger.Net
{
    /// <summary>
    /// Represents the resource listing.
    /// </summary>
    public class ResourceListing
    {
        #region Fields
        private List<ResourceApi> m_apis = new List<ResourceApi>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the API version.
        /// </summary>
        /// <value>
        /// The API version.
        /// </value>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the Swagger version.
        /// </summary>
        /// <value>
        /// The Swagger version.
        /// </value>
        public string SwaggerVersion { get; set; }

        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        /// <value>
        /// The base path.
        /// </value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the resource path.
        /// </summary>
        /// <value>
        /// The resource path.
        /// </value>
        public string ResourcePath { get; set; }

        /// <summary>
        /// Gets or sets the models.
        /// </summary>
        /// <value>
        /// The models.
        /// </value>
        public ResourceModelNodeCollection Models { get; set; }

        /// <summary>
        /// Gets the apis.
        /// </summary>
        public IList<ResourceApi> Apis
        {
            get
            {
                var result = m_apis.OrderBy(a => a.operations.Select(o => o.httpMethod).FirstOrDefault()).ThenBy(a => a.path).ToList().AsReadOnly();

                return result;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds an API to the liting.
        /// </summary>
        /// <param name="api">An API.</param>
        public void AddApi(ResourceApi api)
        {
            m_apis.Add(api);
        }
        #endregion
    }
}
