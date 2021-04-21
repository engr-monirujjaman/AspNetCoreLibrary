
// Single resource
using System;

namespace Microsoft.AspNetCore.Internal
{
    public abstract class IResource : IDisposable
    {
        public abstract void Release(long requestedCount);

        public abstract void Dispose();
    }

    public class RateLimitResource : IResource
    {
        public static RateLimitResource Instance = new RateLimitResource();

        public override void Dispose() { }
        public override void Release(long requestedCount) { }
    }
}
