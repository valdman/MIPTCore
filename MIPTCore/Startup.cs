﻿using System;
using System.Threading.Tasks;
using DataAccess.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MIPTCore.Authentification;
using MIPTCore.Extensions;
using MIPTCore.Middlewares;
using UserManagment;

namespace MIPTCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   
            // Add framework services.
            services.AddMvc()
                .AddFluentValidation()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAntiforgery();
            
            services.AddAuthentication("MIPTCoreCookieAuthenticationScheme")
                .AddCookie("MIPTCoreCookieAuthenticationScheme", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    policyBuilder => policyBuilder.AddRequirements(
                        new IsInRole(UserRole.Admin)));
                options.AddPolicy(
                    "User",
                    policyBuilder => policyBuilder.AddRequirements(
                        new IsAuthentificated()));
            });

            
            //Add DI starter
            new Bootstraper(services, Configuration).Configure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //Domain exception wrapper middleware
            app.DomainErrorHandlingMiddleware();

            EnsureSchemaCreated(app);

            app.UseAuthentication();
            
            app.UseCors(
                options => options.WithOrigins("http://192.168.1.240").AllowAnyMethod()
            );

            app.UseMvc();
        }

        private void EnsureSchemaCreated(IApplicationBuilder app)
        {
            //Ensure schema created
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbCapitalContext = serviceScope.ServiceProvider.GetService<CapitalContext>();
                var dbUsersContext = serviceScope.ServiceProvider.GetService<UserContext>();

                EnsureSchemaFor(dbCapitalContext, dbUsersContext).Wait();
                //ForceCreateTablesFor(dbCapitalContext, dbUsersContext).Wait();
            }
            

            async Task EnsureSchemaFor(params DbContext[] contexts)
            {
                foreach (var dbContext in contexts)
                {
                    await dbContext.Database.EnsureCreatedAsync();
                }
            }
        
            async Task ForceCreateTablesFor(params DbContext[] contexts)
            {
                foreach (var dbContext in contexts)
                {
                    var databaseCreator = 
                        (RelationalDatabaseCreator) dbContext.Database.GetService<IDatabaseCreator>();
                    await databaseCreator.CreateTablesAsync();
                }
            }
        }
       
    }
}