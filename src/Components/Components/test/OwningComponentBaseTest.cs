// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.AspNetCore.Components;

public class OwningComponentBaseTest
{
    [Fact]
    public void CreatesScopeAndService()
    {
        var services = new ServiceCollection();
        services.AddSingleton<Counter>();
        services.AddTransient<MyService>();
        var serviceProvider = services.BuildServiceProvider();

        var counter = serviceProvider.GetRequiredService<Counter>();
        var renderer = new TestRenderer(serviceProvider);
        var component1 = (MyOwningComponent)renderer.InstantiateComponent<MyOwningComponent>();

        Assert.NotNull(component1.MyService);
        Assert.Equal(1, counter.CreatedCount);
        Assert.Equal(0, counter.DisposedCount);

        ((IDisposable)component1).Dispose();
        Assert.Equal(1, counter.CreatedCount);
        Assert.Equal(1, counter.DisposedCount);
    }

    private class Counter
    {
        public int CreatedCount { get; set; }
        public int DisposedCount { get; set; }
    }

    private class MyService : IDisposable
    {
        public MyService(Counter counter)
        {
            Counter = counter;
            Counter.CreatedCount++;
        }

        public Counter Counter { get; }

        void IDisposable.Dispose() => Counter.DisposedCount++;
    }

    private class MyOwningComponent : OwningComponentBase<MyService>
    {
        public MyService MyService => Service;
    }
}
