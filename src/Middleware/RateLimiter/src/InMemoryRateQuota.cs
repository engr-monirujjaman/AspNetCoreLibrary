//using System;
//using System.Collections.Concurrent;
//using System.Diagnostics.CodeAnalysis;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Internal;

//namespace Microsoft.AspNetCore.RateLimiter
//{
//    public class InMemoryRateQuota : IRateQuota
//    {
//        private long _resourceCount;
//        private readonly long _maxResourceCount;
//        private readonly long _newResourcePerSecond;
//        private Timer _renewTimer;
//        private readonly ConcurrentQueue<RateLimitRequest> _queue = new ConcurrentQueue<RateLimitRequest>();

//        public long EstimatedCount => Interlocked.Read(ref _resourceCount);

//        public AggregatedRateLimiter(long resourceCount, long newResourcePerSecond)
//        {
//            _resourceCount = resourceCount;
//            _maxResourceCount = resourceCount; // Another variable for max resource count?
//            _newResourcePerSecond = newResourcePerSecond;

//            // Start timer (5s for demo)
//            _renewTimer = new Timer(Replenish, this, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
//        }

//        public bool TryAcquire(long requestedCount, [NotNullWhen(true)] out IResource? resource)
//        {
//            if (Interlocked.Add(ref _resourceCount, -requestedCount) >= 0)
//            {
//                resource = RateLimitResource.Instance;
//                return true;
//            }

//            Interlocked.Add(ref _resourceCount, requestedCount);
//            resource = null;
//            return false;
//        }

//        public ValueTask<IResource> AcquireAsync(long requestedCount, CancellationToken cancellationToken = default)
//        {
//            if (Interlocked.Add(ref _resourceCount, -requestedCount) >= 0)
//            {
//                return ValueTask.FromResult<IResource>(RateLimitResource.Instance);
//            }

//            Interlocked.Add(ref _resourceCount, requestedCount);

//            var registration = new RateLimitRequest(requestedCount);

//            if (WaitHandle.WaitAny(new[] { registration.MRE.WaitHandle, cancellationToken.WaitHandle }) == 0)
//            {
//                return ValueTask.FromResult<IResource>(RateLimitResource.Instance);
//            }

//            throw new InvalidOperationException("Limit exceeded");
//        }

//        private static void Replenish(object? state)
//        {
//            // Return if Replenish already running to avoid concurrency.
//            var limiter = state as RateLimiter;

//            if (limiter == null)
//            {
//                return;
//            }

//            if (limiter._resourceCount < limiter._maxResourceCount)
//            {
//                var resourceToAdd = Math.Min(limiter._newResourcePerSecond, limiter._maxResourceCount - limiter._resourceCount);
//                Interlocked.Add(ref limiter._resourceCount, resourceToAdd);
//            }

//            // Process queued requests
//            var queue = limiter._queue;
//            while (queue.TryPeek(out var request))
//            {
//                if (Interlocked.Add(ref limiter._resourceCount, -request.Count) >= 0)
//                {
//                    // Request can be fulfilled
//                    queue.TryDequeue(out var requestToFulfill);

//                    if (requestToFulfill == request)
//                    {
//                        // If requestToFulfill == request, the fulfillment is successful.
//                        requestToFulfill.MRE.Set();
//                    }
//                    else
//                    {
//                        // If requestToFulfill != request, there was a concurrent Dequeue:
//                        // 1. Reset the resource count.
//                        // 2. Put requestToFulfill back in the queue (no longer FIFO) if not null
//                        Interlocked.Add(ref limiter._resourceCount, request.Count);
//                        if (requestToFulfill != null)
//                        {
//                            queue.Enqueue(requestToFulfill);
//                        }
//                    }
//                }
//                else
//                {
//                    // Request cannot be fulfilled
//                    Interlocked.Add(ref limiter._resourceCount, request.Count);
//                    break;
//                }
//            }
//        }

//        private class RateLimitRequest
//        {
//            public RateLimitRequest(long count)
//            {
//                Count = count;
//                MRE = new ManualResetEventSlim();
//            }

//            public long Count { get; }

//            public ManualResetEventSlim MRE { get; }
//        }
//    }
//}
