// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Microsoft.AspNetCore.Mvc.Razor;

/// <inheritdoc />
public class TagHelperInitializer<TTagHelper> : ITagHelperInitializer<TTagHelper>
    where TTagHelper : ITagHelper
{
    private readonly Action<TTagHelper, ViewContext> _initializeDelegate;

    /// <summary>
    /// Creates a <see cref="TagHelperInitializer{TTagHelper}"/>.
    /// </summary>
    /// <param name="action">The initialization delegate.</param>
    public TagHelperInitializer(Action<TTagHelper, ViewContext> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        _initializeDelegate = action;
    }

    /// <inheritdoc />
    public void Initialize(TTagHelper helper, ViewContext context)
    {
        if (helper == null)
        {
            throw new ArgumentNullException(nameof(helper));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        _initializeDelegate(helper, context);
    }
}
