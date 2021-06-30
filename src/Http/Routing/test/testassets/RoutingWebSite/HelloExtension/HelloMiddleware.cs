// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace RoutingWebSite.HelloExtension
{
    public class HelloMiddleware
    {
        private readonly HelloOptions _helloOptions;
        private readonly byte[] _helloPayload;

        public HelloMiddleware(RequestDelegate next, IOptions<HelloOptions> helloOptions)
        {
            _helloOptions = helloOptions.Value;

            var payload = new List<byte>();
            payload.AddRange(Encoding.UTF8.GetBytes("Hello"));
            if (!string.IsNullOrEmpty(_helloOptions.Greeter))
            {
                payload.Add((byte)' ');
                payload.AddRange(Encoding.UTF8.GetBytes(_helloOptions.Greeter));
            }
            _helloPayload = payload.ToArray();
        }

        public Task InvokeAsync(HttpContext context)
        {
            var response = context.Response;
            var payloadLength = _helloPayload.Length;
            response.StatusCode = 200;
            response.ContentType = "text/plain";
            response.ContentLength = payloadLength;
            return response.Body.WriteAsync(_helloPayload, 0, payloadLength);
        }
    }
}
