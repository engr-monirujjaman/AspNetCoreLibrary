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
        private readonly List<Action<IServiceCollection>> _operations = new();
        private bool _trackOperations;

        public WebApplicationServiceCollection()
        {
            _serviceCollection = new ServiceCollection();
            _trackOperations = true;
        }

        public ServiceDescriptor this[int index] { get => _serviceCollection[index]; set => _serviceCollection[index] = value; }

        public int Count => _serviceCollection.Count;

        public bool IsReadOnly => _serviceCollection.IsReadOnly;

        public void Add(ServiceDescriptor item)
        {
            _serviceCollection.Add(item);

            if (_trackOperations)
            {
                _operations.Add(sc => sc.Add(item));
            }
        }

        public void Clear()
        {
            _serviceCollection.Clear();

            if (_trackOperations)
            {
                _operations.Add(sc => sc.Clear());
            }
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

            if (_trackOperations)
            {
                _operations.Add(sc => sc.Insert(index, item));
            }
        }

        public bool Remove(ServiceDescriptor item)
        {
            var removed = _serviceCollection.Remove(item);
            if (_trackOperations)
            {
                _operations.Add(sc => sc.Remove(item));
            }
            return removed;
        }

        public void RemoveAt(int index)
        {
            _serviceCollection.RemoveAt(index);
            if (_trackOperations)
            {
                _operations.Add(sc => sc.RemoveAt(index));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<Action<IServiceCollection>> GetOperations()
        {
            if (_operations.Count == 0)
            {
                return _operations;
            }
            // Copy the list
            var operations = _operations.ToList();
            _operations.Clear();
            return operations;
        }

        public void StartTracking()
        {
            _trackOperations = true;
        }

        public void StopTracking()
        {
            _trackOperations = false;
        }
    }
}
