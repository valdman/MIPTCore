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
            var objects = Assembly.GetAssembly(typeof(T))
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(DbContext)))
                .Select(type => (DbContext) Activator.CreateInstance(type, constructorArgs))
                .ToList();
            return objects;
        }
    }
}