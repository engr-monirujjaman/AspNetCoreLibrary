// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Testing;

namespace Microsoft.AspNetCore.Server.IntegrationTesting;

public static class TimeoutExtensions
{
    public static TimeSpan DefaultTimeoutValue = TimeSpan.FromSeconds(300);
}
