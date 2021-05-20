// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.ResourceLimits;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.RequestLimiter
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RequestLimiterOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public RequestLimiterMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IOptions<RequestLimiterOptions> options)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLimiterMiddleware>();
            _options = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Resource limiting: " + context.Request.Path);

            var endpoint = context.GetEndpoint();
            var attributes = endpoint?.Metadata.GetOrderedMetadata<RequestLimitAttribute>();

            if (attributes == null)
            {
                // TODO: Apply default policy
                await _next.Invoke(context);
                return;
            }

            var resources = new Stack<Resource>();
            try
            {
                foreach (var attribute in attributes)
                {
                    if (!string.IsNullOrEmpty(attribute.Policy) && attribute.LimiterRegistration != null)
                    {
                        // TODO: error
                    }

                    if (registration.ResolveLimiter != null)
                    {
                        var limiter = registration.ResolveLimiter(context.RequestServices);
                        _logger.LogInformation("Resource count: " + limiter.EstimatedCount);
                        if (!limiter.TryAcquire(out var resource))
                        {
                            _logger.LogInformation("Resource exhausted");
                            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                            return;
                        }

                        _logger.LogInformation("Resource obtained");
                        resources.Push(resource.Value);// Shouldn't NotNullWhen mean it's never null here?
                    }
                    if (registration.ResolveAggregatedLimiter != null)
                    {
                        var limiter = registration.ResolveAggregatedLimiter(context.RequestServices);
                        _logger.LogInformation("Resource count: " + limiter.EstimatedCount(context));
                        if (!limiter.TryAcquire(context, out var resource))
                        {
                            _logger.LogInformation("Resource exhausted");
                            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                            return;
                        }

                        _logger.LogInformation("Resource obtained");
                        resources.Push(resource.Value);// Shouldn't NotNullWhen mean it's never null here?
                    }

                }

                await _next.Invoke(context);
            }
            finally
            {
                while (resources.TryPop(out var resource))
                {
                    _logger.LogInformation("Releasing resource");
                    resource.Dispose();
                }
            };
        }
    }
}
