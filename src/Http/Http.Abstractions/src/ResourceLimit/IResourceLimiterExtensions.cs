
// Single resource
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Internal
{
    public static class ResourceLimiterExtensions
    {
        
        public static bool TryAcquire(this IResourceLimiter limiter, out Resource resource)
        {
            return limiter.TryAcquire(1, out resource);
        }

        public static ValueTask<Resource> AcquireAsync(this IResourceLimiter limiter, CancellationToken cancellationToken = default)
        {
            return limiter.AcquireAsync(1, cancellationToken);
        }
        public static bool TryAcquire<TKey>(this IAggregatedResourceLimiter<TKey> limiter, TKey resourceId, out Resource resource)
        {
            return limiter.TryAcquire(resourceId, 1, out resource);
        }

        public static ValueTask<Resource> AcquireAsync<TKey>(this IAggregatedResourceLimiter<TKey> limiter, TKey resourceId, CancellationToken cancellationToken = default)
        {
            return limiter.AcquireAsync(resourceId, 1, cancellationToken);
        }
    }
}
