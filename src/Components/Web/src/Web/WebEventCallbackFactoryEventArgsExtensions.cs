// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components.Web;

/// <summary>
/// Provides extension methods for <see cref="EventCallbackFactory"/> and <see cref="EventArgs"/> types.
/// </summary>
public static class WebEventCallbackFactoryEventArgsExtensions
{
    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<ClipboardEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<ClipboardEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<ClipboardEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<ClipboardEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<ClipboardEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<ClipboardEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<DragEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<DragEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<DragEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<DragEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<DragEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<DragEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<ErrorEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<ErrorEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<ErrorEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<ErrorEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<ErrorEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<ErrorEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<FocusEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<FocusEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<FocusEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<FocusEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<FocusEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<FocusEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<KeyboardEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<KeyboardEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<KeyboardEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<KeyboardEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<KeyboardEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<KeyboardEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<MouseEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<MouseEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<MouseEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<MouseEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<MouseEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<MouseEventArgs>(receiver, callback);
    }
    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<PointerEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<PointerEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<PointerEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<PointerEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<PointerEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<PointerEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<ProgressEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<ProgressEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<ProgressEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<ProgressEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<ProgressEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<ProgressEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<TouchEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<TouchEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<TouchEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<TouchEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<TouchEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<TouchEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<WheelEventArgs> Create(this EventCallbackFactory factory, object receiver, Action<WheelEventArgs> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<WheelEventArgs>(receiver, callback);
    }

    /// <summary>
    /// Creates an <see cref="EventCallback"/> for the provided <paramref name="receiver"/> and
    /// <paramref name="callback"/>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/>.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    [Obsolete("This extension method is obsolete and will be removed in a future version. Use the generic overload instead.")]
    public static EventCallback<WheelEventArgs> Create(this EventCallbackFactory factory, object receiver, Func<WheelEventArgs, Task> callback)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return factory.Create<WheelEventArgs>(receiver, callback);
    }
}
