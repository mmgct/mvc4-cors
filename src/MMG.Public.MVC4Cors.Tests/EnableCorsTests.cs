// *************************************************
// MMG.Public.MVCCors.Tests.EnableCorsTests.cs
// Last Modified: 03/16/2016 9:44 AM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVCCors.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Moq;
    using MVC4Cors;
    using NUnit.Framework;

    [TestFixture]
    public class EnableCorsActionFilterTests
    {
        [Test]
        public void TestEnableCorsActionFilter_InitializeWithNull()
        {
            var filter = new CorsEnabledAttribute();
            CollectionAssert.IsEmpty(filter.AllowedDomains);
        }

        [Test]
        public void TestEnableCorsActionFilter_InitializeWithNullString()
        {
            var filter = new CorsEnabledAttribute(String.Empty);
            CollectionAssert.IsEmpty(filter.AllowedDomains);
        }

        [Test]
        public void TestEnableCorsActionFilter_EmptyDomainsAreIgnored()
        {
            var filter = new CorsEnabledAttribute("http://domain1.com,,http://domain3.com");
            Assert.AreEqual(2, filter.AllowedDomains.Count);
        }

        [Test]
        public void TestEnableCorsActionFilter_InitializeBySingleCSVString()
        {
            var filter = new CorsEnabledAttribute("http://domain1.com,http://domain2.com;http://domain3.com");
            CollectionAssert.AreEqual(new[] {"http://domain1.com", "http://domain2.com", "http://domain3.com"}, filter.AllowedDomains);
        }

        private Mock<ActionExecutingContext> getMockedActionExecutingContext(string pOrigin)
        {
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var httpContext = new Mock<HttpContextBase>();
            var actionContext = new Mock<ActionExecutingContext>();

            httpContext.SetupGet(x => x.Request).Returns(request.Object);
            httpContext.SetupGet(x => x.Response).Returns(response.Object);
            response.SetupGet(x => x.Headers).Returns(new WebHeaderCollection());
            response.Setup(r => r.AddHeader(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((x, y) => response.Object.Headers.Add(x, y));
            response.SetupProperty(x => x.StatusCode);
            response.SetupProperty(x => x.StatusDescription);
            request.SetupGet(x => x.Headers).Returns
                (new WebHeaderCollection()
                {
                    {
                        Headers.Origin,
                        pOrigin
                    }
                }
                );
            actionContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
            return actionContext;
        }

        [Test]
        public void TestEnableCorsActionFilter()
        {
            var origin = "http://www.acme.com";
            string allowedDomains = "http://www.acme.com";

            var actionContext = getMockedActionExecutingContext(origin);
            var response = actionContext.Object.HttpContext.Response;
            var filter = new CorsEnabledAttribute(allowedDomains);

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(1, response.Headers.Count);
            var corsHeader = response.Headers["Access-Control-Allow-Origin"];
            Assert.NotNull(corsHeader);
            Assert.AreEqual(origin, corsHeader);
        }

        private class TestAllowableDomains : IAllowableDomains
        {
            public bool IsDomainAllowed(IEnumerable<string> pDomains, string pOrigin)
            {
                return pOrigin == "BOGUS";
            }
        }

        [Test]
        public void TestDelegateFunction()
        {
            var origin = "BOGUS";
            string allowedDomains = "http://www.acme.com";

            var actionContext = getMockedActionExecutingContext(origin);
            var response = actionContext.Object.HttpContext.Response;
            var filter = new CorsEnabledAttribute(new TestAllowableDomains(), allowedDomains);

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(1, response.Headers.Count);
            var corsHeader = response.Headers["Access-Control-Allow-Origin"];
            Assert.NotNull(corsHeader);
            Assert.AreEqual(origin, corsHeader);
        }

        [Test]
        public void TestDelegateFunction_StringInitialize()
        {
            var origin = "BOGUS";
            string allowedDomain = "http://www.acme.com";

            var actionContext = getMockedActionExecutingContext(origin);
            var response = actionContext.Object.HttpContext.Response;
            var filter = new CorsEnabledAttribute(new TestAllowableDomains(), allowedDomain);

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(1, response.Headers.Count);
            var corsHeader = response.Headers["Access-Control-Allow-Origin"];
            Assert.NotNull(corsHeader);
            Assert.AreEqual(origin, corsHeader);
        }

        [Test]
        public void TestEnableCorsActionFilter_DisallowedDomain()
        {
            var origin = "http://www.acme.com";
            string allowedDomains = "";

            var actionContext = getMockedActionExecutingContext(origin);
            var response = actionContext.Object.HttpContext.Response;
            var filter = new CorsEnabledAttribute(allowedDomains);

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(0, response.Headers.Count);
            Assert.AreEqual((int) HttpStatusCode.Forbidden, ((HttpStatusCodeResult) (actionContext.Object.Result)).StatusCode);
            Assert.AreEqual("Failed Cross-Origin Request", ((HttpStatusCodeResult) (actionContext.Object.Result)).StatusDescription);
        }

        [Test]
        public void TestEnableCorsActionFilter_MultipleDomainConfigEntries()
        {
            var origin = "http://www.acme.com";
            string allowedDomains = "http://www.nope.com,http://www.acme.com,http://www.google.com";

            var actionContext = getMockedActionExecutingContext(origin);
            var response = actionContext.Object.HttpContext.Response;
            var filter = new CorsEnabledAttribute(allowedDomains);

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(1, response.Headers.Count);
            var corsHeader = response.Headers["Access-Control-Allow-Origin"];
            Assert.NotNull(corsHeader);
            Assert.AreEqual(origin, corsHeader);
        }

        [Test]
        public void TestEnableCorsActionFilter_MultipleDomainsDenied()
        {
            var origin = "http://www.acme.com";
            string allowedDomains = "http://www.nope.com,http://www.nope2.com,http://www.google.com";

            var actionContext = getMockedActionExecutingContext(origin);
            var response = actionContext.Object.HttpContext.Response;
            var filter = new CorsEnabledAttribute(allowedDomains);

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(0, response.Headers.Count);
        }

        [Test]
        public void TestEnableCorsActionFilter_NoDomainsDefined()
        {
            var origin = "http://www.acme.com";

            var actionContext = getMockedActionExecutingContext(origin);
            var response = actionContext.Object.HttpContext.Response;
            var filter = new CorsEnabledAttribute();

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(0, response.Headers.Count);
        }

        [Test]
        public void TestEnableCorsActionFilter_PreFlight_Put()
        {
            var origin = "http://www.acme.com";
            string[] allowedDomains = {"http://www.nope.com", "http://www.nope2.com", "http://www.google.com"};

            var actionContext = getMockedActionExecutingContext(origin);
            var response = actionContext.Object.HttpContext.Response;
            var filter = new CorsEnabledAttribute();

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(0, response.Headers.Count);
        }
    }
}