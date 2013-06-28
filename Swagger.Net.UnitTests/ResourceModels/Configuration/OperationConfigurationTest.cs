using System;
using System.Net.Http;
using NUnit.Framework;
using Rhino.Mocks;
using Swagger.Net.ResourceModels.Configuration;

namespace Swagger.Net.UnitTests.ResourceModels.Configuration
{
    [TestFixture]
    public class OperationConfigurationTest
    {
        [Test]
        public void IsMapped_HttpHeaderIsEqualToValue_True()
        {
            var proxy = MockRepository.GenerateMock<IConfigurationProxy>();
            proxy.Expect(p => p.GetHeaderValue("name")).Return("v2");
            ConfigurationProvider.Proxy = proxy;

            var target = new OperationConfiguration(HttpMethod.Get, "{id}");
            target.HttpHeader("name").IsEqualToValue("v1", "v2");

            Assert.IsTrue(target.IsMapped(null));

            proxy.VerifyAllExpectations();
        }
        
        [Test]
        public void IsMapped_HttpHeaderIsEqualToValue_False()
        {
            var proxy = MockRepository.GenerateMock<IConfigurationProxy>();
            proxy.Expect(p => p.GetHeaderValue("name")).Return("v3");
            ConfigurationProvider.Proxy = proxy;

            var target = new OperationConfiguration(HttpMethod.Get, "{id}");
            target.HttpHeader("name").IsEqualToValue("v1", "v2");

            Assert.IsFalse(target.IsMapped(null));

            proxy.VerifyAllExpectations();
        }       
    }
}