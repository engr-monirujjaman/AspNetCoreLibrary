// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesWebSite;

[BindProperties(SupportsGet = true)]
public class BindPropertiesWithSupportsGetOnModel : PageModel
{
    public string Property { get; set; }
}
