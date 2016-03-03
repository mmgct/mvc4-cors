// *************************************************
// MMG.Public.MVCCors.Tests.EnableCorsTests.cs
// Last Modified: 03/03/2016 3:33 PM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVCCors.Tests
{
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
        public void TestEnableCorsActionFilter_InitializeBySingleCSVString()
        {
            var filter = new CorsEnabledAttribute("domain1.com,domain2.com;domain3.com");
            CollectionAssert.AreEqual(new[] {"domain1.com", "domain2.com", "domain3.com"}, filter.AllowedDomains);
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
            string[] allowedDomains = {"http://www.acme.com"};

            var actionContext = getMockedActionExecutingContext(origin);
            var response = actionContext.Object.HttpContext.Response;
            var filter = new CorsEnabledAttribute(allowedDomains);

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(1, response.Headers.Count);
            var corsHeader = response.Headers["Access-Control-Allow-Origin"];
            Assert.NotNull(corsHeader);
            Assert.AreEqual("http://www.acme.com", corsHeader);
        }

        [Test]
        public void TestEnableCorsActionFilter_DisallowedDomain()
        {
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var httpContext = new Mock<HttpContextBase>();
            var actionContext = new Mock<ActionExecutingContext>();
            var filter = new CorsEnabledAttribute();

            httpContext.SetupGet(x => x.Request).Returns(request.Object);
            httpContext.SetupGet(x => x.Response).Returns(response.Object);
            actionContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
            response.SetupGet(x => x.Headers).Returns(new WebHeaderCollection());

            response.Setup(r => r.AddHeader(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((x, y) => response.Object.Headers.Add(x, y));

            request.SetupGet(x => x.UserHostName).Returns("www.acme.com");

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(0, response.Object.Headers.Count);
        }

        [Test]
        public void TestEnableCorsActionFilter_MultipleDomainConfigEntries()
        {
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var httpContext = new Mock<HttpContextBase>();
            var actionContext = new Mock<ActionExecutingContext>();
            var filter = new CorsEnabledAttribute("www.nope.com", "www.acme.com", "www.google.com");

            httpContext.SetupGet(x => x.Request).Returns(request.Object);
            httpContext.SetupGet(x => x.Response).Returns(response.Object);
            actionContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
            response.SetupGet(x => x.Headers).Returns(new WebHeaderCollection());

            response.Setup(r => r.AddHeader(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((x, y) => response.Object.Headers.Add(x, y));

            request.SetupGet(x => x.UserHostName).Returns("www.acme.com");

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(1, response.Object.Headers.Count);
            var corsHeader = response.Object.Headers["Access-Control-Allow-Origin"];
            Assert.NotNull(corsHeader);
            Assert.AreEqual("www.acme.com", corsHeader);
        }

        [Test]
        public void TestEnableCorsActionFilter_MultipleDomainsDenied()
        {
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var httpContext = new Mock<HttpContextBase>();
            var actionContext = new Mock<ActionExecutingContext>();
            var filter = new CorsEnabledAttribute("www.nope.com", "www.nope2.com", "www.google.com");

            httpContext.SetupGet(x => x.Request).Returns(request.Object);
            httpContext.SetupGet(x => x.Response).Returns(response.Object);
            actionContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
            response.SetupGet(x => x.Headers).Returns(new WebHeaderCollection());

            response.Setup(r => r.AddHeader(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((x, y) => response.Object.Headers.Add(x, y));

            request.SetupGet(x => x.UserHostName).Returns("www.acme.com");

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(0, response.Object.Headers.Count);
        }

        [Test]
        public void TestEnableCorsActionFilter_NoDomainsDefined()
        {
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var httpContext = new Mock<HttpContextBase>();
            var actionContext = new Mock<ActionExecutingContext>();
            var filter = new CorsEnabledAttribute();

            httpContext.SetupGet(x => x.Request).Returns(request.Object);
            httpContext.SetupGet(x => x.Response).Returns(response.Object);
            actionContext.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
            response.SetupGet(x => x.Headers).Returns(new WebHeaderCollection());

            response.Setup(r => r.AddHeader(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((x, y) => response.Object.Headers.Add(x, y));

            request.SetupGet(x => x.UserHostName).Returns("www.acme.com");

            filter.OnActionExecuting(actionContext.Object);
            Assert.AreEqual(0, response.Object.Headers.Count);
        }
    }
}