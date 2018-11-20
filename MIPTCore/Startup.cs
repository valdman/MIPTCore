using System;
using System.Threading.Tasks;
using DataAccess.Contexts;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using MIPTCore.Authentification;
using MIPTCore.Extensions;
using UserManagment;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

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
                .AddJsonFile("paymentSettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method To add services To the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration =>
            {
                configuration.UsePostgreSqlStorage(Configuration.GetConnectionString("Postgres"));
            });
            
            services.AddMvc()
                .AddFluentValidation()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAntiforgery();

            services.AddAuthentication("MIPTCoreCookieAuthenticationScheme")
                .AddCookie("MIPTCoreCookieAuthenticationScheme", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.FromResult(0);
                    };
                    options.Cookie.SameSite = SameSiteMode.None;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    policyBuilder => policyBuilder.AddRequirements(
                        new IsInRole(UserRole.Admin)));
                options.AddPolicy(
                    "Supervisor",
                    policyBuilder => policyBuilder.AddRequirements(
                        new IsInRole(UserRole.Supervisor)));
                options.AddPolicy(
                    "User",
                    policyBuilder => policyBuilder.AddRequirements(
                        new IsAuthentificated()));
            });

            //Add DI starter
            new Bootstraper(services, Configuration).Configure();
        }

        // This method gets called by the runtime. Use this method To configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //Hangfire Web-admin
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthHandler(env) },
                DisplayStorageConnectionString = false
            });
            app.UseHangfireServer();

            //Middlewares
            app.DomainErrorHandlingMiddleware();

            //EnsureSchemaCreated(app);

            app.UseAuthentication();

            app.UseCors(
                options => options
                    .WithOrigins("http://fund.mipt.ru").AllowAnyMethod().AllowCredentials().AllowAnyHeader()
                    .WithOrigins("http://localhost:3000").AllowAnyMethod().AllowCredentials().AllowAnyHeader()
                    .WithOrigins("http://185.204.0.35:5002").AllowAnyMethod().AllowCredentials().AllowAnyHeader()
            );

            app.UseMvc();
        }

        private void EnsureSchemaCreated(IApplicationBuilder app)
        {
            //Ensure schema created
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbCapitalContext = serviceScope.ServiceProvider.GetService<WithImageContext>();
                var dbUsersContext = serviceScope.ServiceProvider.GetService<UserContext>();
                var dbPageContext = serviceScope.ServiceProvider.GetService<PageContext>();
                var dbTicketContext = serviceScope.ServiceProvider.GetService<TicketContext>();
                var dbDomainOptionsContext = serviceScope.ServiceProvider.GetService<DomainOptionsContext>();

                EnsureSchemaFor(dbCapitalContext, dbUsersContext, dbPageContext, dbTicketContext,
                    dbDomainOptionsContext).Wait();
                //ForceCreateTablesFor(dbFrontendHelperContext).Wait();
            }


            async Task EnsureSchemaFor(params DbContext[] contexts)
            {
                foreach (var dbContext in contexts)
                {
                    await dbContext.Database.EnsureCreatedAsync();
                    await dbContext.Database.MigrateAsync();
                }
            }
        }
    }
}