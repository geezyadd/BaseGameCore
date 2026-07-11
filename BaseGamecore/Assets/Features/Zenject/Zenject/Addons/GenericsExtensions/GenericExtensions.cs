using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace Features.Zenject.Zenject.Addons.GenericsExtensions {
    public static class GenericExtensions {
        public static void BindAllSpecificImplementationsToInterface(this DiContainer container, Assembly targetAssembly, Type interfaceType) {
            foreach (Type type in targetAssembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface || !type.IsClass)
                    continue;
        
                List<Type> persistentInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
                    .ToList();
        
                foreach (var iface in persistentInterfaces)
                    container.Bind(iface).To(type).AsSingle();
            }
        }
    }
}