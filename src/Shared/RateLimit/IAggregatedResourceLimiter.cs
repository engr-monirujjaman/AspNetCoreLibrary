
// Single resource
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Internal
{
    // Represent an aggregated resource (e.g. a resource limiter aggregated by IP)
    public interface IAggregatedResourceLimiter<TKey>
    {
        // an inaccurate view of resources
        long EstimatedCount(TKey resourceID);

        // Fast synchronous attempt to acquire resources
        bool TryAcquire(TKey resourceID, long requestedCount, out IResource? resource);

        // Wait until the requested resources are available
        // If unsuccessful, throw
        ValueTask<IResource> AcquireAsync(TKey resourceID, long requestedCount, CancellationToken cancellationToken = default);
    }
}
