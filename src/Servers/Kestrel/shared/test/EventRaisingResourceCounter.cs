// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace Microsoft.AspNetCore.Server.Kestrel.Tests
{
    internal class EventRaisingResourceLimiter : IResourceLimiter
    {
        private readonly IResourceLimiter _wrapped;

        public EventRaisingResourceLimiter(IResourceLimiter wrapped)
        {
            _wrapped = wrapped;
        }

        public long EstimatedCount => _wrapped.EstimatedCount;

        public event EventHandler OnRelease;
        public event EventHandler<bool> OnLock;

        public ValueTask<IResource> AcquireAsync(long requestedCount, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool TryAcquire(long requestedCount, out IResource resource)
        {
            var retVal = _wrapped.TryAcquire(out var innerResource);
            resource = new WrappedResource(innerResource, () => OnRelease.Invoke(this, null));
            OnLock?.Invoke(this, retVal);
            return retVal;
        }

        private class WrappedResource : IResource
        {
            private readonly IResource _resource;
            private readonly Action _releaseAction;

            public WrappedResource(IResource resource, Action releaseAction)
            {
                _resource = resource;
                _releaseAction = releaseAction;
            }

            public override void Release(long requestedCount)
            {
                _resource.Release(requestedCount);
                _releaseAction();
            }

            // Careful with locking in dispose
            public override void Dispose()
            {
                _resource.Release(_resource.Count);
            }
        }
    }
}
