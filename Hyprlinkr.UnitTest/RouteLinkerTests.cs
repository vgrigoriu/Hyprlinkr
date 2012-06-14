﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.Hyprlinkr;
using System.Linq.Expressions;
using Ploeh.Hyprlinkr.UnitTest.Controllers;
using Ploeh.AutoFixture.Xunit;
using System.Net.Http;

namespace Ploeh.Hyprlinkr.UnitTest
{
    public class RouteLinkerTests
    {
        [Theory, AutoHypData]
        public void GetNullThrows(RouteLinker sut)
        {
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetUri<Version>(null));
        }

        [Theory, AutoHypData]
        public void GetUriFromInvalidExpressionThrows(RouteLinker sut)
        {
            Expression<Action<object>> expressionWithIsNotAMethodCall = _ => new object();
            Assert.Throws<ArgumentException>(() =>
                sut.GetUri<object>(expressionWithIsNotAMethodCall));
        }

        [Theory, AutoHypData]
        public void GetRouteForDefaultGetMethodReturnsCorrectResult(
            [Frozen]HttpRequestMessage request,
            RouteLinker sut)
        {
            Uri actual = sut.GetUri<FooController>(r => r.Get());

            var baseUri = request.RequestUri.GetLeftPart(UriPartial.Authority);
            var expected = new Uri(new Uri(baseUri), "Foo/");
            Assert.Equal(expected, actual);
        }
    }
}