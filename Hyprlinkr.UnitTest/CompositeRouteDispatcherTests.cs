using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Hyprlinkr.UnitTest
{
    public class CompositeRouteDispatcherTests
    {
        [Theory, AutoHypData]
        public void SutHasAppropriateGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(CompositeRouteDispatcher));
        }

        [Theory, AutoHypData]
        public void SutReturnsRouplesFromInnerRouteDispatchers(
            [Frozen]Mock<IRouteDispatcher> firstDispatcherStub,
            [Frozen]Mock<IRouteDispatcher> secondDispatcherStub,
            [Frozen]Rouple firstRouple,
            [Frozen]Rouple seconRouple,
            CompositeRouteDispatcher sut)
        {
            // Fixture setup
            Expression<Action<CompositeRouteDispatcherTests>> exp =
                c => c.SutHasAppropriateGuards(null);
            var methodCallExp = (MethodCallExpression)exp.Body;

            firstDispatcherStub
                .Setup(d => d.Dispatch(
                    It.IsAny<MethodCallExpression>(),
                    It.IsAny<IDictionary<string, object>>()))
                .Returns(new[] {firstRouple});
            secondDispatcherStub
                .Setup(d => d.Dispatch(
                    It.IsAny<MethodCallExpression>(),
                    It.IsAny<IDictionary<string, object>>()))
                .Returns(new[] { seconRouple });

            var actual = sut.Dispatch(methodCallExp, new Dictionary<string, object>());

            Assert.Contains(firstRouple, actual);
            Assert.Contains(seconRouple, actual);
        }
    }
}
