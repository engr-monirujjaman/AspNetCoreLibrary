using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Internal;

namespace Microsoft.AspNetCore.RateLimiter
{
    class RateLimitRegistration
    {
        internal Func<IServiceProvider, IResourceLimiter>? ResolveLimiter { get; }
        internal Func<IServiceProvider, IAggregatedResourceLimiter<HttpContext>>? ResolveAggregatedLimiter { get; }

        public RateLimitRegistration(Func<IServiceProvider, IResourceLimiter> resolveLimiter)
        {
            ResolveLimiter = resolveLimiter;
        }

        public RateLimitRegistration(Func<IServiceProvider, IAggregatedResourceLimiter<HttpContext>> resolveAggregatedLimiter)
        {
            ResolveAggregatedLimiter = resolveAggregatedLimiter;
        }
    }
}
