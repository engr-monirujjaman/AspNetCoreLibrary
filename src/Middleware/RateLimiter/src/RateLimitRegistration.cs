using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Internal;

namespace Microsoft.AspNetCore.RateLimiter
{
    class RateLimitRegistration
    {
        internal Func<IServiceProvider, ResourceLimiter>? ResolveLimiter { get; }
        internal Func<IServiceProvider, AggregatedResourceLimiter<HttpContext>>? ResolveAggregatedLimiter { get; }

        public RateLimitRegistration(Func<IServiceProvider, ResourceLimiter> resolveLimiter)
        {
            ResolveLimiter = resolveLimiter;
        }

        public RateLimitRegistration(Func<IServiceProvider, AggregatedResourceLimiter<HttpContext>> resolveAggregatedLimiter)
        {
            ResolveAggregatedLimiter = resolveAggregatedLimiter;
        }
    }
}
