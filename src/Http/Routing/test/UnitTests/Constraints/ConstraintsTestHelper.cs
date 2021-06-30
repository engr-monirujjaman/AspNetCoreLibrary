// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Routing.Tests
{
    public class ConstraintsTestHelper
    {
        public static bool TestConstraint(IRouteConstraint constraint, object value)
        {
            var parameterName = "fake";
            var values = new RouteValueDictionary() { { parameterName, value } };
            var routeDirection = RouteDirection.IncomingRequest;
            return constraint.Match(httpContext: null, route: null, parameterName, values, routeDirection);
        }
    }
}
