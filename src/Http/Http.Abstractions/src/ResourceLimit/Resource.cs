
// Single resource
using System;

namespace Microsoft.AspNetCore.Internal
{
    public struct Resource : IDisposable
    {
        public object? State { get; init; }

        private Action<Resource>? _onDispose;

        public Resource(object? state, Action<Resource>? onDispose)
        {
            State = state;
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose?.Invoke(this);
        }

        public static Resource NoopResource = new Resource(null, null);
    }
}
