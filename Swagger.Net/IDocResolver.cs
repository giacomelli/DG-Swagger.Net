using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Swagger.Net
{
    public interface IDocumentationResolver
    {
        string GetDocumentation(string suggestedDocumentation, MemberInfo member);
    }
}
