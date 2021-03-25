// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.AspNetCore.Components.Reflection
{
    internal static class ComponentPropertiesWithIMagicFoo
    {
        public static void SetProperties(in ParameterView parameters, IMagicFooBar target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            // The logic is split up for simplicity now that we have CaptureUnmatchedValues parameters.
            if (!target.TryGetUnmatchedValuesWriter(out var unmatchedWriter))
            {
                // Logic for components without a CaptureUnmatchedValues parameter
                foreach (var parameter in parameters)
                {
                    var parameterName = parameter.Name;

                    if (!target.TryGetWriter(parameterName, out var writer))
                    {
                        // Case 1: There is nowhere to put this value.
                        ThrowForUnknownIncomingParameterName(target.GetType(), parameterName);
                        throw null; // Unreachable
                    }
                    else if (writer.Cascading && !parameter.Cascading)
                    {
                        // We don't allow you to set a cascading parameter with a non-cascading value. Put another way:
                        // cascading parameters are not part of the public API of a component, so it's not reasonable
                        // for someone to set it directly.
                        //
                        // If we find a strong reason for this to work in the future we can reverse our decision since
                        // this throws today.
                        ThrowForSettingCascadingParameterWithNonCascadingValue(target.GetType(), parameterName);
                        throw null; // Unreachable
                    }
                    else if (!writer.Cascading && parameter.Cascading)
                    {
                        // We're giving a more specific error here because trying to set a non-cascading parameter
                        // with a cascading value is likely deliberate (but not supported), or is a bug in our code.
                        ThrowForSettingParameterWithCascadingValue(target.GetType(), parameterName);
                        throw null; // Unreachable
                    }

                    SetProperty(target, writer, parameterName, parameter);
                }
            }
            else
            {
                // Logic with components with a CaptureUnmatchedValues parameter
                var isCaptureUnmatchedValuesParameterSetExplicitly = false;
                Dictionary<string, object>? unmatched = null;
                foreach (var parameter in parameters)
                {
                    var parameterName = parameter.Name;

                    if (target.TryGetWriter(parameterName, out var writer))
                    {
                        if (writer.CapturesUnmatchedValues)
                        {
                            if (parameter.Cascading)
                            {
                                // Don't allow an "extra" cascading value to be collected - or don't allow a non-cascading
                                // parameter to be set with a cascading value.
                                //
                                // This is likely a bug in our infrastructure or an attempt to deliberately do something unsupported.
                                ThrowForSettingParameterWithCascadingValue(target.GetType(), parameterName);
                                throw null; // Unreachable
                            }

                            isCaptureUnmatchedValuesParameterSetExplicitly = true;
                        }

                        if (!writer.Cascading && parameter.Cascading)
                        {
                            // Don't allow an "extra" cascading value to be collected - or don't allow a non-cascading
                            // parameter to be set with a cascading value.
                            //
                            // This is likely a bug in our infrastructure or an attempt to deliberately do something unsupported.
                            ThrowForSettingParameterWithCascadingValue(target.GetType(), parameterName);
                            throw null; // Unreachable
                        }
                        else if (writer.Cascading && !parameter.Cascading)
                        {
                            // Allow unmatched parameters to collide with the names of cascading parameters. This is
                            // valid because cascading parameter names are not part of the public API. There's no
                            // way for the user of a component to know what the names of cascading parameters
                            // are.
                            unmatched ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                            unmatched[parameterName] = parameter.Value;
                        }
                        else
                        {
                            SetProperty(target, writer, parameterName, parameter.Value);
                        }
                    }
                    else
                    {
                        unmatched ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                        unmatched[parameterName] = parameter.Value;
                    }
                }

                if (unmatched != null && isCaptureUnmatchedValuesParameterSetExplicitly)
                {
                    // This has to be an error because we want to allow users to set the CaptureUnmatchedValues
                    // parameter explicitly and ....
                    // 1. We don't ever want to mutate a value the user gives us.
                    // 2. We also don't want to implicitly copy a value the user gives us.
                    //
                    // Either one of those implementation choices would do something unexpected.
                    ThrowForCaptureUnmatchedValuesConflict(target.GetType(), unmatched);
                    throw null; // Unreachable
                }
                else if (unmatched != null)
                {

                    SetProperty(target, unmatchedWriter, nameof(ParameterAttribute.CaptureUnmatchedValues), unmatched);
                }
            }

            static void SetProperty(object target, ParameterWriter writer, string name, object value)
            {
                try
                {
                    writer.SetValue(value);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Unable to set property '{name}' on object of " +
                        $"type '{target.GetType().FullName}'. The error was: {ex.Message}", ex);
                }
            }
        }

        [DoesNotReturn]
        private static void ThrowForUnknownIncomingParameterName(Type targetType, string parameterName)
        {
            // We know we're going to throw by this stage, so it doesn't matter that the following
            // reflection code will be slow. We're just trying to help developers see what they did wrong.
            throw new InvalidOperationException(
                $"Object of type '{targetType.FullName}' does not have a parameter matching the name '{parameterName}'. Parameters must be public properties annotated with [Parameter] attribute.");
        }

        [DoesNotReturn]
        private static void ThrowForSettingCascadingParameterWithNonCascadingValue(Type targetType, string parameterName)
        {
            throw new InvalidOperationException(
                $"Object of type '{targetType.FullName}' has a property matching the name '{parameterName}', " +
                $"but it does not have [{nameof(ParameterAttribute)}] applied.");
        }

        [DoesNotReturn]
        private static void ThrowForSettingParameterWithCascadingValue(Type targetType, string parameterName)
        {
            throw new InvalidOperationException(
                $"The property '{parameterName}' on component type '{targetType.FullName}' cannot be set " +
                $"using a cascading value.");
        }

        [DoesNotReturn]
        private static void ThrowForCaptureUnmatchedValuesConflict(Type targetType, Dictionary<string, object> unmatched)
        {
            throw new InvalidOperationException(
                $"Property with CapturedUnmatchedValues=true on component type '{targetType.FullName}' cannot be set explicitly " +
                $"when also used to capture unmatched values. Unmatched values:" + Environment.NewLine +
                string.Join(Environment.NewLine, unmatched.Keys.OrderBy(k => k)));
        }
    }
}
