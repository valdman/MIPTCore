using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace SchemaCreator
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2 || args[0] != "-c")
            {
                Console.WriteLine("usage: dotnet SchemaCreator.dll -c <ConnetctionString>");
                return 1;
            }

            var options = new DbContextOptionsBuilder();
            options.UseNpgsql(args[1]);
            
            var dbContexts = ReflectiveEnumerator
                .GetDbContextsFromAssemblyWith<UserRepository>(options.Options).ToArray();

            var errors = new List<Exception>();

            foreach (var dbContext in dbContexts)
            {
                try
                {
                    ForceCreateTablesFor(dbContext).Wait();
                    Console.WriteLine($"{dbContext.GetType().Name} schema created");
                }
                catch (Exception e)
                {
                    errors.Add(e);
                }
            }

            if (errors.Count <= 0) return 0;
            foreach (var exception in errors)
            {
                Console.WriteLine(exception.Message);
            }
            return 1;
        }
        
        private static async Task ForceCreateTablesFor(DbContext dbContext)
        {
            var databaseCreator = 
                (RelationalDatabaseCreator) dbContext.Database.GetService<IDatabaseCreator>();
            await databaseCreator.CreateTablesAsync();
        }
    }
}