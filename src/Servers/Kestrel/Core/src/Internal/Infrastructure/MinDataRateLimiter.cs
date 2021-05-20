using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.ResourceLimits;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure
{
    internal class MinDataRateLimiter : ResourceLimiter
    {
        private long _bytesTokens;
        private MinDataRate _minDataRate;
        private long _initialTimestamp;
        private long _lastTimestamp;
        private double _rateRemainder;

        public MinDataRateLimiter(MinDataRate minDataRate, long initialTimestamp, EventHandler<long>? tickEvent)
        {
            _minDataRate = minDataRate;
            _initialTimestamp = initialTimestamp;
            _lastTimestamp = initialTimestamp;
            tickEvent += (sender, tick) =>
            {
                Tick(tick); // need to unsubscribe
            };
        }

        public override long EstimatedCount {
            get {
                if (Interlocked.Read(ref _lastTimestamp) - _initialTimestamp <= _minDataRate.GracePeriod.Ticks)
                {
                    return long.MaxValue;
                }

                return Interlocked.Read(ref _bytesTokens);
            }
        }

        public override ValueTask<Resource> AcquireAsync(long requestedCount, CancellationToken cancellationToken = default)
        {
            Interlocked.Add(ref _bytesTokens, requestedCount);

            return ValueTask.FromResult(Resource.NoopResource);
        }

        public override bool TryAcquire(long requestedCount, [NotNullWhen(true)] out Resource? resource)
        {
            resource = Resource.NoopResource;

            Interlocked.Add(ref _bytesTokens, requestedCount);

            return true;
        }

        internal void Tick(long timestamp)
        {
            // Noop if less than 1 second has elapsed
            var elapsedTicks = timestamp - Interlocked.Read(ref _lastTimestamp);
            if (elapsedTicks < TimeSpan.TicksPerSecond)
            {
                return;
            }

            // No need to update byte counts count is non-positive
            if (Interlocked.Read(ref _bytesTokens) <= 0)
            {
                _rateRemainder = 0.0;
                Interlocked.Exchange(ref _lastTimestamp, timestamp);
                return;
            }

            // Assume overly long tick intervals are the result of server resource starvation.
            // Don't count extra time between ticks against the rate limit.
            var bytes = _minDataRate.BytesPerSecond*(Math.Min(Heartbeat.Interval.Ticks, elapsedTicks)/TimeSpan.TicksPerSecond) + _rateRemainder;
            var bytesToDecrement = (long)bytes;
            _rateRemainder = bytes - bytesToDecrement;

            var newTokens = Interlocked.Add(ref _bytesTokens, -bytesToDecrement);

            if (newTokens < 0)
            {
                Interlocked.Add(ref _bytesTokens, Math.Min(-newTokens, bytesToDecrement));
            }

            Interlocked.Exchange(ref _lastTimestamp, timestamp);
        }
    }
}
