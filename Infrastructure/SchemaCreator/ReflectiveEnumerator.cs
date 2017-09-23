using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace SchemaCreator
{
    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<DbContext> GetDbContextsFromAssemblyWith<T>(params object[] constructorArgs) where T : class
        {
            var typesOfContexts = Assembly.GetAssembly(typeof(T))
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(DbContext))).ToArray();

            var constructors =
                typesOfContexts.Select(type => type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).First());
            var objectsOfTheseTypes = constructors.Select(ctor => (DbContext)ctor.Invoke(constructorArgs));
            return objectsOfTheseTypes;
        }
    }
}