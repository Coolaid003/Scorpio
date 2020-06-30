﻿using System.Collections.Concurrent;
using System.Threading;

using Scorpio.DependencyInjection;

namespace Scorpio.Runtime
{
    internal class AsyncLocalAmbientDataContext : IAmbientDataContext, ISingletonDependency
    {
        private static readonly ConcurrentDictionary<string, AsyncLocal<object>> _asyncLocalDictionary = new ConcurrentDictionary<string, AsyncLocal<object>>();

        public void SetData(string key, object value)
        {
            var asyncLocal = _asyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
            asyncLocal.Value = value;
        }

        public object GetData(string key)
        {
            var asyncLocal = _asyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
            return asyncLocal.Value;
        }

    }
}
