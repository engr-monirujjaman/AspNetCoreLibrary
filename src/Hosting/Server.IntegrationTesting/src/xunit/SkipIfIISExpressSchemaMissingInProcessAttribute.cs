// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Testing;

namespace Microsoft.AspNetCore.Server.IntegrationTesting;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly | AttributeTargets.Class)]
public sealed partial class SkipIfIISExpressSchemaMissingInProcessAttribute : Attribute, ITestCondition
{
    public bool IsMet => IISExpressAncmSchema.SupportsInProcessHosting;

    public string SkipReason => IISExpressAncmSchema.SkipReason;
}
