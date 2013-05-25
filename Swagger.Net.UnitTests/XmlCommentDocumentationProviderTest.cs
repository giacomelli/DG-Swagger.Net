using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Swagger.Net.UnitTests
{
	[TestFixture]
	public class XmlCommentDocumentationProviderTest
	{
		[Test]
		public void ProcessTypeName_GenericList_SpecList()
		{
			var typeName = "System.Collections.Generic.List`1[[Terra.Adv.AdApi.Domain.LineItems.LineItem, Terra.Adv.AdApi.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]";
			var actual = XmlCommentDocumentationProvider.ProcessTypeName(typeName);

			Assert.AreEqual("System.Collections.Generic.List{Terra.Adv.AdApi.Domain.LineItems.LineItem}", actual);
		}
	}
}
