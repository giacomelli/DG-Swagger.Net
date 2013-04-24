using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Xml.XPath;

namespace Swagger.Net
{
	/// <summary>
	/// Accesses the XML doc blocks written in code to further document the API.
	/// All credit goes to: <see cref="http://blogs.msdn.com/b/yaohuang1/archive/2012/05/21/asp-net-web-api-generating-a-web-api-help-page-using-apiexplorer.aspx"/>
	/// </summary>
	public class XmlCommentDocumentationProvider : IDocumentationProvider
	{
		XPathNavigator _documentNavigator;
		private const string _methodExpression = "/doc/members/member[@name='M:{0}']";
		private static Regex nullableTypeNameRegex = new Regex(@"(.*\.Nullable)" + Regex.Escape("`1[[") + "([^,]*),.*");

		public XmlCommentDocumentationProvider(string documentPath)
		{
			XPathDocument xpath = new XPathDocument(documentPath);
			_documentNavigator = xpath.CreateNavigator();
		}

		public virtual string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
		{
			ReflectedHttpParameterDescriptor reflectedParameterDescriptor = parameterDescriptor as ReflectedHttpParameterDescriptor;
			
			if (reflectedParameterDescriptor != null)
			{
				XPathNavigator memberNode = GetMemberNode(reflectedParameterDescriptor.ActionDescriptor);
				
				if (memberNode != null)
				{
					string parameterName = reflectedParameterDescriptor.ParameterInfo.Name;
					XPathNavigator parameterNode = memberNode.SelectSingleNode(string.Format("param[@name='{0}']", parameterName));
				
					if (parameterNode != null)
					{
						return parameterNode.Value.Trim();
					}
				}
			}

			return "No Documentation Found.";
		}

		public virtual bool GetRequired(HttpParameterDescriptor parameterDescriptor)
		{
			ReflectedHttpParameterDescriptor reflectedParameterDescriptor = parameterDescriptor as ReflectedHttpParameterDescriptor;
			if (reflectedParameterDescriptor != null)
			{
				return !reflectedParameterDescriptor.ParameterInfo.IsOptional;
			}

			return true;
		}

		public virtual string GetDocumentation(HttpActionDescriptor actionDescriptor)
		{
			XPathNavigator memberNode = GetMemberNode(actionDescriptor);
			if (memberNode != null)
			{
				XPathNavigator summaryNode = memberNode.SelectSingleNode("summary");
				if (summaryNode != null)
				{
					return summaryNode.Value.Trim();
				}
			}

			return "No Documentation Found.";
		}

		public virtual string GetNotes(HttpActionDescriptor actionDescriptor)
		{
			XPathNavigator memberNode = GetMemberNode(actionDescriptor);
			if (memberNode != null)
			{
				XPathNavigator summaryNode = memberNode.SelectSingleNode("remarks");
				if (summaryNode != null)
				{
					return summaryNode.Value.Trim();
				}
			}

			return "No Documentation Found.";
		}

		public virtual IList<ResourceApiOperationParameterErrorResponse> GetErrorResponses(HttpActionDescriptor actionDescriptor)
		{
			var result = new List<ResourceApiOperationParameterErrorResponse>();
			var memberNode = GetMemberNode(actionDescriptor);
		
			if (memberNode != null)
			{
				var navigator = memberNode.Select("exception");

				foreach (XPathNavigator n in navigator)
				{
					var errorResponse = new ResourceApiOperationParameterErrorResponse();
					errorResponse.Code = n.GetAttribute("code", "");
					errorResponse.Reason = n.GetAttribute("message", "");

					if (String.IsNullOrEmpty(errorResponse.Reason))
					{
						errorResponse.Reason = n.GetAttribute("reason", "");
					}

					result.Add(errorResponse);
				}
			}

			return result;
		}

		public virtual string GetResponseClass(HttpActionDescriptor actionDescriptor)
		{
			var reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;

			if (reflectedActionDescriptor != null)
			{
				if (reflectedActionDescriptor.MethodInfo.ReturnType.IsGenericType)
				{
					StringBuilder sb = new StringBuilder(reflectedActionDescriptor.MethodInfo.ReturnParameter.ParameterType.Name);
					sb.Append("<");
					Type[] types = reflectedActionDescriptor.MethodInfo.ReturnParameter.ParameterType.GetGenericArguments();
					for(int i = 0; i < types.Length; i++)
					{
						sb.Append(types[i].Name);
						if(i != (types.Length - 1)) sb.Append(", ");
					}
					sb.Append(">");
					return sb.Replace("`1","").ToString();
				}
				else
					return reflectedActionDescriptor.MethodInfo.ReturnType.Name;
			}

			return "void";
		}

		public virtual string GetNickname(HttpActionDescriptor actionDescriptor)
		{
			ReflectedHttpActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
			if (reflectedActionDescriptor != null)
			{
				return reflectedActionDescriptor.MethodInfo.Name;
			}

			return "NicknameNotFound";
		}

		private XPathNavigator GetMemberNode(HttpActionDescriptor actionDescriptor)
		{
			ReflectedHttpActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
			if (reflectedActionDescriptor != null)
			{
				string selectExpression = string.Format(_methodExpression, GetMemberName(reflectedActionDescriptor.MethodInfo));
				XPathNavigator node = _documentNavigator.SelectSingleNode(selectExpression);
				if (node != null)
				{
					return node;
				}
			}

			return null;
		}

		private static string GetMemberName(MethodInfo method)
		{
			string name = string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
			var parameters = method.GetParameters();
			if (parameters.Length != 0)
			{
				string[] parameterTypeNames = parameters.Select(param => ProcessTypeName(param.ParameterType.FullName)).ToArray();
				name += string.Format("({0})", string.Join(",", parameterTypeNames));
			}

			return name;
		}

		public static string ProcessTypeName(string typeName)
		{
			//handle nullable
			var result = nullableTypeNameRegex.Match(typeName);
			if (result.Success)
			{
				return string.Format("{0}{{{1}}}", result.Groups[1].Value, result.Groups[2].Value);
			}

			result = Regex.Match(typeName, @"System\.Collections\.Generic\.List\`1\[\[([A-Z0-9_\.]+)", RegexOptions.IgnoreCase);

			if (result.Success)
			{
				return string.Format(@"System.Collections.Generic.List{{{0}}}", result.Groups[1].Value);
			}

			return typeName;
		}
	}
}
