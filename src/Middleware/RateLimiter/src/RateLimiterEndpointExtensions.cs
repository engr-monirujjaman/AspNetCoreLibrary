// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.RateLimiter
{
    /// <summary>
    /// 
    /// </summary>
    public static class RateLimiterEndpointExtensions
    {
        public static IEndpointConventionBuilder EnforceLimit(this IEndpointConventionBuilder builder, long requestPerSecond)
        {
            var limiter = new RateLimiter(requestPerSecond, requestPerSecond);

            builder.Add(endpointBuilder =>
            {
                endpointBuilder.Metadata.Add(new RateLimitRegistration(services => limiter));
            });

            return builder;
        }

        public static IEndpointConventionBuilder EnforceLimit<TResourceLimiter>(this IEndpointConventionBuilder builder)
            where TResourceLimiter : ResourceLimiter
        {
            builder.Add(endpointBuilder =>
            {
                endpointBuilder.Metadata.Add(new RateLimitRegistration(services => services.GetRequiredService<TResourceLimiter>()));
            });
            return builder;
        }

        public static IEndpointConventionBuilder EnforceAggregatedLimit<TAggregatedResourceLimiter>(this IEndpointConventionBuilder builder)
            where TAggregatedResourceLimiter : AggregatedResourceLimiter<HttpContext>
        {
            builder.Add(endpointBuilder =>
            {
                endpointBuilder.Metadata.Add(new RateLimitRegistration(services => services.GetRequiredService<TAggregatedResourceLimiter>()));
            });
            return builder;
        }
    }
}
