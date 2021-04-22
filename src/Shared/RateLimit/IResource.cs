
// Single resource
using System;

namespace Microsoft.AspNetCore.Internal
{
    public abstract class IResource : IDisposable
    {
        public long Count { get; init; }

        public abstract void Release(long requestedCount);

        public abstract void Dispose();
    }

    public class RateLimitNoopResource : IResource
    {
        public static RateLimitNoopResource Instance = new RateLimitNoopResource { Count = 0 };

        public override void Dispose() { }
        public override void Release(long requestedCount) { }
    }
}
