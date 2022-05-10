using System;

namespace Common.Dependency
{
    public static class ServiceLocator
    {
        private static ServiceLocatorProvider _currentProvider;

        public static IServiceProvider Current
        {
            get
            {
                if (!IsLocationProviderSet)
                    throw new InvalidOperationException(nameof(_currentProvider));

                return _currentProvider();
            }
        }

        public static void SetLocatorProvider(ServiceLocatorProvider newProvider)
        {
            _currentProvider = newProvider;
        }

        public static bool IsLocationProviderSet => _currentProvider != null;
    }
}
