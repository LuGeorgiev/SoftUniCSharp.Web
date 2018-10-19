using System;

namespace Services.Contracts
{
    public interface IServiceCollection
    {
        void AddService<TSource, TDestination>();

        T CreateInstance<T>();

        object CreateInstance(Type type);

        void AddService<T>(Func<T> func);
    }
}
