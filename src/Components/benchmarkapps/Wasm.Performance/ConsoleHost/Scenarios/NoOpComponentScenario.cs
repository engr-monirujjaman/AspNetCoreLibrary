// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Wasm.Performance.ConsoleHost.Scenarios
{
    class NoOpComponentScenario : ComponentRenderingScenarioBase
    {
        public NoOpComponentScenario() : base("noop")
        {
        }

        protected override async Task ExecuteAsync(ConsoleHostRenderer renderer, int numCycles)
        {
            for (var i = 0; i < numCycles; i++)
            {
                var hostPage = new NoOpHostPage();
                var componentId = renderer.AssignRootComponentId(hostPage);
                await renderer.RenderRootComponentAsync(componentId);
                await renderer.RenderRootComponentAsync(componentId);
            }
        }

        class NoOpHostPage : ComponentBase
        {
            private static object paramValue = new object();

            protected override void BuildRenderTree(RenderTreeBuilder builder)
            {
                for (var i = 0; i < 4000; i++)
                {
                    builder.OpenComponent<NoOpComponent>(0);
                    builder.AddAttribute(1, nameof(NoOpComponent.Param1), paramValue);
                    builder.AddAttribute(2, nameof(NoOpComponent.Param2), paramValue);
                    builder.AddAttribute(3, nameof(NoOpComponent.Param3), paramValue);
                    builder.CloseComponent();
                }
            }
        }

        class NoOpComponent : ComponentBase
        {

            [Parameter] public object Param1 { get; set; }
            [Parameter] public object Param2 { get; set; }
            [Parameter] public object Param3 { get; set; }

            public override Task SetParametersAsync(ParameterView parameters)
            {
                foreach (var param in parameters)
                {
                    switch (param.Name[5])
                    {
                        case '1':
                            Param1 = param.Value;
                            break;
                        case '2':
                            Param2 = param.Value;
                            break;
                        case '3':
                            Param3 = param.Value;
                            break;
                    }
                }

                StateHasChanged();
                return Task.CompletedTask;
            }
        }
    }
}
