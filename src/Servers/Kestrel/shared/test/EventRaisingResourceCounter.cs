// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Internal;

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

        public ValueTask<Resource> AcquireAsync(long requestedCount, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool TryAcquire(long requestedCount, out Resource resource)
        {
            resource = Resource.NoopResource;
            var retVal = _wrapped.TryAcquire(out var innerResource);
            if (retVal)
            {
                resource = new Resource(innerResource.State, r => OnRelease.Invoke(this, null));
            }
            OnLock?.Invoke(this, retVal);
            return retVal;
        }
    }
}
