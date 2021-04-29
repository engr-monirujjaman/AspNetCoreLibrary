
// Single resource
using System;

namespace Microsoft.AspNetCore.Internal
{
    public struct Resource : IDisposable
    {
        public long Count { get; init; }
        public object? State { get; init; }

        private Action<Resource>? _onDispose;

        public Resource(long count, object? state, Action<Resource>? onDispose)
        {
            Count = count;
            State = state;
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose?.Invoke(this);
        }

        public static Resource NoopResource = new Resource(0, null, null);
    }
}
