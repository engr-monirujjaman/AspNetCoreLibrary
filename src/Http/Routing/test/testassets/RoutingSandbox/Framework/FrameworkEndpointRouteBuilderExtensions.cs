// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace RoutingSandbox.Framework
{
    public static class FrameworkEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapFramework(this IEndpointRouteBuilder endpoints, Action<FrameworkConfigurationBuilder> configure)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var dataSource = endpoints.ServiceProvider.GetRequiredService<FrameworkEndpointDataSource>();

            var configurationBuilder = new FrameworkConfigurationBuilder(dataSource);
            configure(configurationBuilder);

            endpoints.DataSources.Add(dataSource);

            return dataSource;
        }
    }
}
