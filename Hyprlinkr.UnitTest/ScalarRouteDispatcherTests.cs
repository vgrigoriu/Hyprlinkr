﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Ploeh.Hyprlinkr;
using Xunit;
using System.Reflection;
using Ploeh.Hyprlinkr.UnitTest.Controllers;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using System.Linq.Expressions;

namespace Ploeh.Hyprlinkr.UnitTest
{
    public class ScalarRouteDispatcherTests
    {
        [Theory, AutoHypData]
        public void SutHasAppropriateGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(DefaultRouteDispatcher));
        }

        [Theory, AutoHypData]
        public void SutIsRouteLinker(DefaultRouteDispatcher sut)
        {
            Assert.IsAssignableFrom<IRouteDispatcher>(sut);
        }

        [Theory, AutoHypData]
        public void RouteNameIsCorrect(
            [Frozen]string expected,
            [Greedy]DefaultRouteDispatcher sut)
        {
            Assert.Equal<string>(expected, sut.RouteName);
        }

        [Theory, AutoHypData]
        public void DefaultRouteNameIsCorrect(
            [Modest]DefaultRouteDispatcher sut)
        {
            Assert.Equal("DefaultApi", sut.RouteName);
        }

        [Theory, AutoHypData]
        public void DispatchReturnsResultWithCorrectRouteName(
            [Modest]DefaultRouteDispatcher sut,
            MethodCallExpression method,
            IDictionary<string, object> routeValues)
        {
            var actual = sut.Dispatch(method, routeValues);
            Assert.Equal("DefaultApi", actual.First().RouteName);
        }

        [Theory, AutoHypData]
        public void DispatchReturnsResultWithCustomRouteName(
            [Greedy]DefaultRouteDispatcher sut,
            MethodCallExpression method,
            IDictionary<string, object> routeValues)
        {
            var actual = sut.Dispatch(method, routeValues);
            Assert.Equal(sut.RouteName, actual.First().RouteName);
        }

        [Theory, AutoHypData]
        public void DispatchPreservesAllRouteValues(
            DefaultRouteDispatcher sut,
            MethodCallExpression method,
            IDictionary<string, object> routeValues)
        {
            var actual = sut.Dispatch(method, routeValues);

            var expected = new HashSet<KeyValuePair<string, object>>(routeValues);
            Assert.True(expected.IsSubsetOf(actual.First().RouteValues));
        }

        [Theory, AutoHypData]
        public void DispatchAddsFooControllerNameToRouteValues(
            DefaultRouteDispatcher sut,
            IDictionary<string, object> routeValues)
        {
            Expression<Action<FooController>> exp = c => c.GetDefault();
            var method = (MethodCallExpression)exp.Body;
            var actual = sut.Dispatch(method, routeValues);
            Assert.Equal("foo", actual.First().RouteValues["controller"]);
        }

        [Theory, AutoHypData]
        public void DispatchAddsBarControllerNameToRouteValues(
            DefaultRouteDispatcher sut,
            IDictionary<string, object> routeValues)
        {
            Expression<Action<BarController>> exp = c => c.GetDefault();
            var method = (MethodCallExpression)exp.Body;
            var actual = sut.Dispatch(method, routeValues);
            Assert.Equal("bar", actual.First().RouteValues["controller"]);
        }

        [Theory, AutoHypData]
        public void DispatchDoesNotMutateInputRouteValues(
            DefaultRouteDispatcher sut,
            MethodCallExpression method,
            IDictionary<string, object> routeValues)
        {
            var expected = routeValues.ToList();
            sut.Dispatch(method, routeValues);
            Assert.True(expected.SequenceEqual(routeValues));
        }
    }
}
