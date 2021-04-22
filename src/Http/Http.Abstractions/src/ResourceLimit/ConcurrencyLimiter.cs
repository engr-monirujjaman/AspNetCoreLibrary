using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Internal;

namespace Microsoft.AspNetCore.RateLimiter
{
    public class ConcurrencyLimiter : IResourceLimiter
    {
        private long _resourceCount;
        private readonly long _maxResourceCount;
        private object _lock = new object();
        private ManualResetEventSlim _mre; // How about a FIFO queue instead of randomness?

        // an inaccurate view of resources
        public long EstimatedCount => Interlocked.Read(ref _resourceCount);

        public ConcurrencyLimiter(long resourceCount)
        {
            _resourceCount = resourceCount;
            _maxResourceCount = resourceCount;
            _mre = new ManualResetEventSlim();
        }

        // Fast synchronous attempt to acquire resources
        public bool TryAcquire(long requestedCount, [NotNullWhen(true)] out Resource? resource)
        {
            resource = null;

            if (requestedCount < 0 || requestedCount > _maxResourceCount)
            {
                return false;
            }

            if (requestedCount == 0)
            {
                // TODO check if resources are exhausted
            }

            if (EstimatedCount >= requestedCount)
            {
                lock (_lock) // Check lock check
                {
                    if (EstimatedCount >= requestedCount)
                    {
                        Interlocked.Add(ref _resourceCount, -requestedCount);
                        resource = new ConcurrentResource(this, requestedCount);
                        return true;
                    }
                }
            }

            return false;
        }

        // Wait until the requested resources are available
        public ValueTask<Resource> AcquireAsync(long requestedCount, CancellationToken cancellationToken = default)
        {
            if (requestedCount < 0 || requestedCount > _maxResourceCount)
            {
                throw new InvalidOperationException("Limit exceeded");
            }

            if (EstimatedCount >= requestedCount)
            {
                lock (_lock) // Check lock check
                {
                    if (EstimatedCount >= requestedCount)
                    {
                        Interlocked.Add(ref _resourceCount, -requestedCount);
                        return ValueTask.FromResult<Resource>(
                            new ConcurrentResource(this, requestedCount));
                    }
                }
            }

            // Handle cancellation
            while (true)
            {
                _mre.Wait(cancellationToken); // Handle cancellation

                lock (_lock)
                {
                    if (_mre.IsSet)
                    {
                        _mre.Reset();
                    }

                    if (EstimatedCount > requestedCount)
                    {
                        Interlocked.Add(ref _resourceCount, -requestedCount);
                        return ValueTask.FromResult<Resource>(
                            new ConcurrentResource(this, requestedCount));
                    }
                }
            }
        }

        private void Release(long releaseCount)
        {
            // Check for negative requestCount
            Interlocked.Add(ref _resourceCount, releaseCount);
            _mre.Set();
        }

        private class ConcurrentResource : Resource
        {
            private long _count;
            private ConcurrencyLimiter _limiter;
            private object _lock = new object();

            public ConcurrentResource(ConcurrencyLimiter limiter, long count)
            {
                _count = count;
                _limiter = limiter;
            }

            public override void Release(long requestedCount)
            {
                lock (_lock)
                {
                    var releaseCount = Math.Min(requestedCount, _count);
                    _limiter.Release(releaseCount);
                }
            }

            // Careful with locking in dispose
            public override void Dispose()
            {
                lock (_lock)
                {
                    _limiter.Release(_count);
                }
            }
        }
    }
}
