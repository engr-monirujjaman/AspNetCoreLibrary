
// Single resource
using System;

namespace Microsoft.AspNetCore.Internal
{
    public struct Resource : IDisposable
    {
        public object? State { get; init; }

        private Action<object?>? _onDispose;

        public Resource(object? state, Action<object?>? onDispose)
        {
            State = state;
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose?.Invoke(State);
        }

        public static Resource NoopResource = new Resource(null, null);
    }
}
