using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Swagger.Net
{
    public class FuncDocumentationResolver : IDocumentationResolver
    {
        #region Fields
        private Func<string, MemberInfo, string> m_getDocumentationFunc;
        #endregion

        #region Constructors
        public FuncDocumentationResolver(Func<string, MemberInfo, string> getDocumentationFunc)
        {
            m_getDocumentationFunc = getDocumentationFunc;
        }
        #endregion

        #region Methods
        public string GetDocumentation(string suggestedDocumentation, MemberInfo member)
        {
            return m_getDocumentationFunc(suggestedDocumentation, member);
        }
        #endregion
    }
}
