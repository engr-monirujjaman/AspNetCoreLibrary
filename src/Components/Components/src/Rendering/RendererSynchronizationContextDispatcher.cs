// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components.Rendering;

internal class RendererSynchronizationContextDispatcher : Dispatcher
{
    private readonly RendererSynchronizationContext _context;

    public RendererSynchronizationContextDispatcher()
    {
        _context = new RendererSynchronizationContext();
        _context.UnhandledException += (sender, e) =>
        {
            OnUnhandledException(e);
        };
    }

    public override bool CheckAccess() => SynchronizationContext.Current == _context;

    public override Task InvokeAsync(Action workItem)
    {
        if (CheckAccess())
        {
            workItem();
            return Task.CompletedTask;
        }

        return _context.InvokeAsync(workItem);
    }

    public override Task InvokeAsync(Func<Task> workItem)
    {
        if (CheckAccess())
        {
            return workItem();
        }

        return _context.InvokeAsync(workItem);
    }

    public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
    {
        if (CheckAccess())
        {
            return Task.FromResult(workItem());
        }

        return _context.InvokeAsync<TResult>(workItem);
    }

    public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
    {
        if (CheckAccess())
        {
            return workItem();
        }

        return _context.InvokeAsync<TResult>(workItem);
    }
}
