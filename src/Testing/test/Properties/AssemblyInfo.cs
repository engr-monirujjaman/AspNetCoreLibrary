// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Xunit;

[assembly: Repeat(1)]
[assembly: LogLevel(LogLevel.Trace)]
[assembly: AssemblyFixture(typeof(TestAssemblyFixture))]
