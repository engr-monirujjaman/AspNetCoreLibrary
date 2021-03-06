// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Identity.DefaultUI.WebSite;

/// <summary>
/// Provides version hash for a specified file.
/// </summary>
internal class FileVersionProvider : IFileVersionProvider
{
    public string AddFileVersionToPath(PathString requestPathBase, string path) => path;
}
