// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.ResourceLimits;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace Microsoft.AspNetCore.Server.Kestrel.Core.Internal
{
    internal class ConnectionLimitMiddleware<T> where T : BaseConnectionContext
    {
        private readonly Func<T, Task> _next;
        private readonly IKestrelTrace _trace;
        private readonly ResourceLimiter _limiter;

        public ConnectionLimitMiddleware(Func<T, Task> next, long connectionLimit, IKestrelTrace trace)
            : this(next, new ConcurrencyLimiter(connectionLimit), trace)
        {
        }

        // For Testing
        internal ConnectionLimitMiddleware(Func<T, Task> next, ResourceLimiter limiter, IKestrelTrace trace)
        {
            _next = next;
            _limiter = limiter;
            _trace = trace;
        }

        public async Task OnConnectionAsync(T connection)
        {
            if (!_limiter.TryAcquire(out var resource))
            {
                KestrelEventSource.Log.ConnectionRejected(connection.ConnectionId);
                _trace.ConnectionRejected(connection.ConnectionId);
                await connection.DisposeAsync();
                return;
            }

            // Do we need IDecrementConcurrentConnectionCountFeature?
            var releasor = new ConnectionReleasor(resource.Value);

            try
            {
                connection.Features.Set<IDecrementConcurrentConnectionCountFeature>(releasor);
                await _next(connection);
            }
            finally
            {
                releasor.ReleaseConnection();
            }
        }

        private class ConnectionReleasor : IDecrementConcurrentConnectionCountFeature
        {
            private readonly Resource _resource;

            public ConnectionReleasor(Resource resource)
            {
                _resource = resource;
            }

            public void ReleaseConnection()
            {
                _resource.Dispose();
            }
        }
    }
}
