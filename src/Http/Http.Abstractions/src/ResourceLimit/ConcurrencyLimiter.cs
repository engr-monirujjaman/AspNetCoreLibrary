using System;
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
        public bool TryAcquire(long requestedCount, out Resource resource)
        {
            resource = Resource.NoopResource;

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
                        resource = new Resource(
                            state: null,
                            onDispose: resource => Release(requestedCount));
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
                        return ValueTask.FromResult(new Resource(
                            state: null,
                            onDispose: resource => Release(requestedCount)));
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
                        return ValueTask.FromResult(new Resource(
                            state: null,
                            onDispose: resource => Release(requestedCount)));
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
    }
}
