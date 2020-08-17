// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpSys.Internal;
using Microsoft.AspNetCore.Server.IIS.Core.IO;

namespace Microsoft.AspNetCore.Server.IIS.Core.IO
{
    internal class Http2AsyncIOEngine : IAsyncIOEngine
    {
        private object _contextLock;
        private IntPtr _handler;

        private Http2WriteOperation _cachedHttp2WriteOperation;

        private Http2ReadOperation _cachedHttp2ReadOperation;
        private Http2FlushOperation _cachedHttp2FlushOperation;

        public Http2AsyncIOEngine(object contextLock, IntPtr pInProcessHandler)
        {
            _contextLock = contextLock;
            _handler = pInProcessHandler;
        }

        public void Dispose()
        {
            lock (_contextLock)
            {
                NativeMethods.HttpTryCancelIO(_handler);
            }
        }

        public ValueTask FlushAsync(bool moreData)
        {
            lock (_contextLock)
            {
                var flush = GetFlushOperation();
                flush.Initialize(_handler, moreData);
                flush.Invoke();
                return new ValueTask(flush, 0);
            }
        }

        public void NotifyCompletion(int hr, int bytes)
        {
            // Shouldn't be called
        }

        public ValueTask<int> ReadAsync(Memory<byte> memory)
        {
            lock (_contextLock)
            {
                var read = GetReadOperation();
                read.Initialize(_handler, memory);
                read.Invoke();
                return new ValueTask<int>(read, 0);
            }
        }

        public ValueTask<int> WriteAsync(ReadOnlySequence<byte> data)
        {
            lock (_contextLock)
            {
                var write = GetWriteOperation();
                write.Initialize(_handler, data);
                write.Invoke();
                return new ValueTask<int>(write, 0);
            }
        }

        private Http2ReadOperation GetReadOperation() =>
            Interlocked.Exchange(ref _cachedHttp2ReadOperation, null) ??
            new Http2ReadOperation(this);
        private Http2WriteOperation GetWriteOperation() =>
            Interlocked.Exchange(ref _cachedHttp2WriteOperation, null) ??
            new Http2WriteOperation(this);
        private Http2FlushOperation GetFlushOperation() =>
            Interlocked.Exchange(ref _cachedHttp2FlushOperation, null) ??
            new Http2FlushOperation(this);

        private void ReturnOperation(Http2WriteOperation operation) =>
            Volatile.Write(ref _cachedHttp2WriteOperation, operation);

        private void ReturnOperation(Http2ReadOperation operation) =>
            Volatile.Write(ref _cachedHttp2ReadOperation, operation);

        private void ReturnOperation(Http2FlushOperation operation) =>
            Volatile.Write(ref _cachedHttp2FlushOperation, operation);


        internal sealed class Http2WriteOperation : AsyncWriteOperationBase
        {
            private static readonly NativeMethods.PFN_WEBSOCKET_ASYNC_COMPLETION WriteCallback = (httpContext, completionInfo, completionContext) =>
            {
                var context = (Http2WriteOperation)GCHandle.FromIntPtr(completionContext).Target;

                NativeMethods.HttpGetCompletionInfo(completionInfo, out var cbBytes, out var hr);

                var continuation = context.Complete(hr, cbBytes);
                continuation.Invoke();

                return NativeMethods.REQUEST_NOTIFICATION_STATUS.RQ_NOTIFICATION_PENDING;
            };

            private readonly Http2AsyncIOEngine _engine;
            private GCHandle _thisHandle;

            public Http2WriteOperation(Http2AsyncIOEngine engine)
            {
                _engine = engine;
            }

            protected override unsafe int WriteChunks(IntPtr requestHandler, int chunkCount, HttpApiTypes.HTTP_DATA_CHUNK* dataChunks, out bool completionExpected)
            {
                _thisHandle = GCHandle.Alloc(this);
                return NativeMethods.HttpWebsocketsWriteBytes(requestHandler, dataChunks, chunkCount, WriteCallback, fMoreData: true, (IntPtr)_thisHandle, out completionExpected);
            }

            protected override void ResetOperation()
            {
                base.ResetOperation();
                _thisHandle.Free();
                _engine.ReturnOperation(this);
            }
        }

        internal sealed class Http2FlushOperation : AsyncIOOperation
        {
            private static readonly NativeMethods.PFN_WEBSOCKET_ASYNC_COMPLETION FlushCallback = (httpContext, completionInfo, completionContext) =>
            {
                var context = (Http2FlushOperation)GCHandle.FromIntPtr(completionContext).Target;

                NativeMethods.HttpGetCompletionInfo(completionInfo, out var cbBytes, out var hr);
                if (hr != 0)
                {
                    Console.WriteLine("invoke operation failed");
                }
                var continuation = context.Complete(hr, cbBytes);
                continuation.Invoke();

                return NativeMethods.REQUEST_NOTIFICATION_STATUS.RQ_NOTIFICATION_PENDING;
            };

            private readonly Http2AsyncIOEngine _engine;
            private GCHandle _thisHandle;
            private IntPtr _requestHandler;
            private bool _moreData;

            public Http2FlushOperation(Http2AsyncIOEngine engine)
            {
                _engine = engine;
            }

            public void Initialize(IntPtr requestHandler, bool moreData)
            {
                _requestHandler = requestHandler;
                _moreData = moreData;
            }

            protected override bool InvokeOperation(out int hr, out int bytes)
            {
                bytes = 0;
                _thisHandle = GCHandle.Alloc(this);

                hr = NativeMethods.HttpWebsocketsFlushBytes(_requestHandler, FlushCallback, _moreData, (IntPtr)_thisHandle, out var fCompletionExpected);
                if (hr != 0)
                {
                    Console.WriteLine("invoke operation failed");
                }
                return !fCompletionExpected;
            }

            protected override void ResetOperation()
            {
                base.ResetOperation();
                _thisHandle.Free();
                _engine.ReturnOperation(this);
            }
        }

        internal class Http2ReadOperation : AsyncIOOperation
        {
            public static readonly NativeMethods.PFN_WEBSOCKET_ASYNC_COMPLETION ReadCallback = (httpContext, completionInfo, completionContext) =>
            {
                var context = (Http2ReadOperation)GCHandle.FromIntPtr(completionContext).Target;

                NativeMethods.HttpGetCompletionInfo(completionInfo, out var cbBytes, out var hr);
                if (hr != 0)
                {
                    Console.WriteLine("invoke operation failed");
                }
                var continuation = context.Complete(hr, cbBytes);

                continuation.Invoke();

                return NativeMethods.REQUEST_NOTIFICATION_STATUS.RQ_NOTIFICATION_PENDING;
            };

            private readonly Http2AsyncIOEngine _engine;
            private GCHandle _thisHandle;
            private MemoryHandle _inputHandle;
            private IntPtr _requestHandler;
            private Memory<byte> _memory;

            public Http2ReadOperation(Http2AsyncIOEngine engine)
            {
                _engine = engine;
            }

            protected override unsafe bool InvokeOperation(out int hr, out int bytes)
            {
                _thisHandle = GCHandle.Alloc(this);
                _inputHandle = _memory.Pin();

                hr = NativeMethods.HttpWebsocketsReadBytes(
                    _requestHandler,
                    (byte*)_inputHandle.Pointer,
                    _memory.Length,
                    ReadCallback,
                    (IntPtr)_thisHandle,
                    out bytes,
                    out var completionExpected);
                if (hr != 0)
                {
                    Console.WriteLine("invoke operation failed");
                }
                return !completionExpected;
            }

            public void Initialize(IntPtr requestHandler, Memory<byte> memory)
            {
                _requestHandler = requestHandler;
                _memory = memory;
            }

            public override void FreeOperationResources(int hr, int bytes)
            {
                _inputHandle.Dispose();
            }

            protected override void ResetOperation()
            {
                base.ResetOperation();

                _thisHandle.Free();

                _memory = default;
                _inputHandle.Dispose();
                _inputHandle = default;
                _requestHandler = default;

                _engine.ReturnOperation(this);
            }

            protected override bool IsSuccessfulResult(int hr) => hr == NativeMethods.ERROR_HANDLE_EOF;
        }
    }
}
