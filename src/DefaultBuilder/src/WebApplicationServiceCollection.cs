// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore
{
    internal class WebApplicationServiceCollection : IServiceCollection
    {
        private readonly IServiceCollection _serviceCollection;

        public WebApplicationServiceCollection(ServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public ServiceDescriptor this[int index] { get => _serviceCollection[index]; set => _serviceCollection[index] = value; }

        public int Count => _serviceCollection.Count;

        public bool IsReadOnly => _serviceCollection.IsReadOnly;

        public void Add(ServiceDescriptor item)
        {
            _serviceCollection.Add(item);
        }

        public void Clear()
        {
            _serviceCollection.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return _serviceCollection.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            _serviceCollection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return _serviceCollection.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return _serviceCollection.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            _serviceCollection.Insert(index, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            return _serviceCollection.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _serviceCollection.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
