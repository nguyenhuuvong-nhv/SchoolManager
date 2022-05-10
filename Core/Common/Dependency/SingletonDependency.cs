﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Dependency
{
    public static class SingletonDependency<T>
    {
        public static T Instance => LazyInstance.Value;

        private static readonly Lazy<T> LazyInstance;

        static SingletonDependency()
        {
            LazyInstance = new Lazy<T>(() => ServiceLocator.Current.GetService<T>(), true);
        }
    }
}
