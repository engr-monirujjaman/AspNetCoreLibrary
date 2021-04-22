
// Single resource
using System;

namespace Microsoft.AspNetCore.Internal
{
    public class Resource : IDisposable
    {
        public long Count { get; init; }
        // public object State?

        public virtual void Dispose() { }
        public virtual void Release(long requestedCount) { }

        // This would be returned by rate limiters
        public static Resource NoopResource = new Resource { Count = 0 };
    }
}
