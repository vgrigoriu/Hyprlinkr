using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ploeh.Hyprlinkr
{
    /// <summary>
    /// A composite of other <see cref="IRouteDispatcher"/>
    /// </summary>
    public class CompositeRouteDispatcher: IRouteDispatcher
    {
        private readonly IEnumerable<IRouteDispatcher> innerDispatchers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeRouteDispatcher" /> class.
        /// </summary>
        /// <param name="innerDispatchers">The collection of inner dispatchers</param>
        public CompositeRouteDispatcher(IEnumerable<IRouteDispatcher> innerDispatchers)
        {
            if (innerDispatchers == null) throw new ArgumentNullException("innerDispatchers");
            this.innerDispatchers = innerDispatchers;
        }

        /// <summary>
        /// Provides dispatch information from all the inner dispatchers.
        /// </summary>
        /// <param name="method">The method expression.</param>
        /// <param name="routeValues">Route values.</param>
        /// <returns>All the <see cref="Rouple"/> instances returned by the inner route dispatchers.</returns>
        public IEnumerable<Rouple> Dispatch(MethodCallExpression method, IDictionary<string, object> routeValues)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (routeValues == null) throw new ArgumentNullException("routeValues");

            return innerDispatchers.SelectMany(inner => inner.Dispatch(method, routeValues));
        }
    }
}
