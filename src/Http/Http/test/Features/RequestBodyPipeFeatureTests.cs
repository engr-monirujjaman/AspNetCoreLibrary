// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.AspNetCore.Http.Features;

public class RequestBodyPipeFeatureTests
{
    [Fact]
    public void RequestBodyReturnsStreamPipeReader()
    {
        var context = new DefaultHttpContext();
        var expectedStream = new MemoryStream();
        context.Request.Body = expectedStream;

        var feature = new RequestBodyPipeFeature(context);

        var pipeBody = feature.Reader;

        Assert.NotNull(pipeBody);
    }

    [Fact]
    public async Task RequestBodyGetsDataFromSecondStream()
    {
        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes("hahaha"));
        var feature = new RequestBodyPipeFeature(context);
        var _ = feature.Reader;

        var expectedString = "abcdef";
        context.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes(expectedString));
        var data = await feature.Reader.ReadAsync();
        Assert.Equal(expectedString, GetStringFromReadResult(data));
    }

    private static string GetStringFromReadResult(ReadResult data)
    {
        return Encoding.ASCII.GetString(data.Buffer.ToArray());
    }
}
