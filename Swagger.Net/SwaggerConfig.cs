using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger.Net
{
    /// <summary>
    /// Swagger config.
    /// </summary>
    public static class SwaggerConfig
    {
        #region Constructors
        static SwaggerConfig()
        {
            DocumentationResolver = new DefaultDocumentationResolver();
        }
        #endregion

        #region Properties
        public static IDocumentationResolver DocumentationResolver { get; set; }
        #endregion
    }
}
