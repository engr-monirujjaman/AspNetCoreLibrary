
// Single resource
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Internal
{
    internal interface IRateLimiter
    {
        // an inaccurate view of resources
        long EstimatedCount { get; }

        // Fast synchronous attempt to acquire resources, it won't actually acquire the resource
        bool TryAcquire(long requestedCount);

        // Wait until the requested resources are available
        ValueTask<bool> AcquireAsync(long requestedCount, CancellationToken cancellationToken = default);
    }
}
