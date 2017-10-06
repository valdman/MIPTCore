using AutoMapper;
using CapitalManagment;
using CapitalManagment.Infrastructure;
using CapitalsTableHelper;
using Common;
using Common.Infrastructure;
using DataAccess.Contexts;
using DataAccess.Repositories;
using FileManagment;
using Mailer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIPTCore.Authentification.Handlers;
using MIPTCore.Middlewares;
using MIPTCore.Models;
using PagesManagment;
using PagesManagment.Infrastructure;
using UserManagment;
using UserManagment.Application;
using UserManagment.Infrastructure;

namespace MIPTCore
{
    public class Bootstraper
    {
        private readonly IServiceCollection _services;
        private readonly IConfigurationRoot _configuration;

        public Bootstraper(IServiceCollection services, IConfigurationRoot configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public void Configure()
        {
            ConfigureDatebase();
            ConfigureAutoMapper();

            _services
                //Register auth middleware
                .AddSingleton<IAuthorizationHandler, IsAuthentificatedAuthHandler>()
                .AddSingleton<IAuthorizationHandler, IsInRoleRoleAuthHandler>()
                //Register other middlewares
                .AddScoped<ErrorHandlingMiddleware>()
                //Register settings
                .Configure<BackendSettings>(_configuration.GetSection("BackendSettings"))
                .Configure<MailerSettings>(_configuration.GetSection("MailerSettings"))
                
                //RegisterDomain
                .AddScoped<IUserManager, UserManager>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthentificationService, AuthentificationService>()
                .AddScoped<ITicketSender, TicketSender>()
                .AddScoped<ITicketService, TicketService>()
                .AddScoped<IUserMailerService, UserMailerService>()
                .AddScoped<ITicketSender, TicketSender>()
                .AddScoped<ICapitalManager, CapitalManager>()
                .AddScoped<IImageResizer, ImageResizer>()
                .AddScoped<IFileManager, FileManager>()
                .AddScoped<IPageManager, PageManager>()
                .AddScoped<ICapitalsTableHelper, CapitalsTableHelper.CapitalsTableHelper>();

            _services.Configure<FileStorageSettings>(_configuration.GetSection("FileStorageSettings"));
        }

        private void ConfigureAutoMapper()
        {
            //AutoMapper
            _services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<User, UserModel>();
                cfg.CreateMap<UserRegistrationModel, User>()
                    .ForMember(t => t.Password, o => o.ResolveUsing(p => new Password(p.Password)));
                cfg.CreateMap<UserUpdateModel, User>();
                cfg.CreateMap<AlumniProfile, AlumniProfileModel>();
                cfg.CreateMap<AlumniProfileModel, AlumniProfile>();                
                cfg.CreateMap<CredentialsModel, Credentials>();


                cfg.CreateMap<CapitalUpdatingModel, Capital>();
                cfg.CreateMap<CapitalCreatingModel, Capital>();
                cfg.CreateMap<Capital, CapitalModel>();

                cfg.CreateMap<Image, ImageModel>();
                cfg.CreateMap<ImageModel, Image>();
                
                cfg.CreateMap<PersonModel, Person>();
                cfg.CreateMap<Person, PersonModel>();

                cfg.CreateMap<Page, PageModel>();
                cfg.CreateMap<PageUpdateModel, Page>();
                cfg.CreateMap<PageCreationModel, Page>();
                
                cfg.CreateMap<CapitalsTableEntry, CapitalsTableEntryModel>();
                cfg.CreateMap<CapitalsTableEntryModel, CapitalsTableEntry>();
            });
            
        }

        private void ConfigureDatebase()
        {
            var connectionString = _configuration.GetConnectionString("Postgres");
            
            _services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<DomainOptionsContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<UserContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<CapitalContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<PageContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<TicketContext>(options => options.UseNpgsql(connectionString))

                .AddScoped<IGenericRepository<User>, UserRepository>()
                .AddScoped<IGenericRepository<Capital>, CapitalRepository>()
                .AddScoped<ICapitalsTableEntryRepository, CapitalsTableRepository>()
                .AddScoped<ICapitalRepository, CapitalRepository>()
                .AddScoped<IDomainOptionsRepository, DomainOptionsRepository>()
                .AddScoped<IPageRepository, PageRepository>()
                .AddScoped<ITicketRepository, TicketRepository>();
        }
    }
}