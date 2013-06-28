using System;
using System.Net.Http;
using NUnit.Framework;
using Rhino.Mocks;
using Swagger.Net.ResourceModels.Configuration;

namespace Swagger.Net.UnitTests.ResourceModels.Configuration
{
    [TestFixture]
    public class ResourceConfigurationTest
    {
        [Test]
        public void IsMapped_HttpHeaderIsEqualToValue_True()
        {
            var proxy = MockRepository.GenerateMock<IConfigurationProxy>();
            proxy.Expect(p => p.GetHeaderValue("name")).Return("v2");
            ConfigurationProvider.Proxy = proxy;

            var target = MockRepository.GenerateMock<ResourceConfiguration<string>>();            
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

            var target = MockRepository.GenerateMock<ResourceConfiguration<string>>();
            target.HttpHeader("name").IsEqualToValue("v1", "v2");

            Assert.IsFalse(target.IsMapped(null));

            proxy.VerifyAllExpectations();
        }
               
        [Test]
        public void IsOperationMapped_HttpHeaderIsNotEqualToValue_TrueAndFalse()
        {
            var proxy = MockRepository.GenerateMock<IConfigurationProxy>();
            proxy.Expect(p => p.GetHeaderValue("name")).Return("v2");
            ConfigurationProvider.Proxy = proxy;

            var target = MockRepository.GenerateMock<ResourceConfiguration<string>>();

            target.AllOperations.HttpHeader("name").IsNotEqualToValue("v2", "v3");
            target.Operation(HttpMethod.Get, "{id}").HttpHeader("name").IsNotEqualToValue("v3");
            target.Operation(HttpMethod.Put, "{id}").HttpHeader("name").IsNotEqualToValue("v3");
            target.Operation(HttpMethod.Post, "").HttpHeader("name").IsNotEqualToValue("v2");	

            var targetInterface = (IResourceConfiguration)target;
            Assert.IsFalse(targetInterface.IsOperationMapped(HttpMethod.Get, ""));
            Assert.IsTrue(targetInterface.IsOperationMapped(HttpMethod.Get, "{id}"));
            Assert.IsTrue(targetInterface.IsOperationMapped(HttpMethod.Put, "{id}"));
            Assert.IsFalse(targetInterface.IsOperationMapped(HttpMethod.Post, ""));

            proxy.VerifyAllExpectations();
        }
    }
}