using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Swagger.Net
{
    public class DefaultDocumentationResolver : IDocumentationResolver
    {
        public string GetDocumentation(string suggestedDocumentation, MemberInfo member)
        {
            return suggestedDocumentation;
        }
    }
}
