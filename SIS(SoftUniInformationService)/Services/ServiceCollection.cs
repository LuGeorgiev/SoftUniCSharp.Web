namespace Services
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ServiceCollection : IServiceCollection
    {
        private IDictionary<Type, Type> dependencyContainer;

        public ServiceCollection()
        {
            this.dependencyContainer = new Dictionary<Type, Type>();
        }

        public void AddService<TSource, TDestination>()
        {            
            this.dependencyContainer[typeof(TSource)]= typeof(TDestination);
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));            
        }

        public  object CreateInstance(Type type)
        {
            
            //1. if this. type is registerd return typeof(T)
            if (dependencyContainer.ContainsKey(type))
            {
                type = dependencyContainer[type];
            }
            
                //2. if not registered -> create instance
               

            if (type.IsInterface || type.IsAbstract)
            {
                throw new Exception($"Type {type.FullName} cannot be instantiated");
            }

            var constructor = type.GetConstructors()
                .OrderBy(x=>x.GetParameters().Length)
                .First();
            var constructorParameters = constructor.GetParameters();

            //3. if anoter dependencies are needed -> Recursive instantiation needed
            var constructorParameterObjects = new List<object>();
            foreach (var constructorParameter in constructorParameters)
            {
                var parameterObject = this.CreateInstance(constructorParameter.ParameterType);
                constructorParameterObjects.Add(parameterObject);
            }
            var obj = constructor.Invoke(constructorParameterObjects.ToArray());

            return obj;
        }
    }
}
