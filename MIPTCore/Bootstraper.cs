﻿using AutoMapper;
using BannerHelper;
using CapitalManagment;
using CapitalManagment.Infrastructure;
using CapitalsTableHelper;
using Common.DomainSteroids;
using Common.Entities;
using Common.Infrastructure;
using DataAccess.Contexts;
using DataAccess.Repositories;
using DonationManagment;
using DonationManagment.Application;
using DonationManagment.Infrastructure;
using FileManagment;
using Loggly;
using Loggly.Config;
using Mailer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIPTCore.Authentification.Handlers;
using MIPTCore.Middlewares;
using MIPTCore.Models;
using MIPTCore.Settings;
using NavigationHelper;
using NewsManagment;
using PagesManagment;
using PagesManagment.Infrastructure;
using PaymentGateway;
using StoriesManagment;
using UserManagment;
using UserManagment.Application;
using UserManagment.Infrastructure;
using UserReadModel;
using MailerImpl = Mailer.Mailer;

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
            ConfigureLoggly();

            _services
                //Register auth middleware
                .AddSingleton<IAuthorizationHandler, IsAuthentificatedAuthHandler>()
                .AddSingleton<IAuthorizationHandler, IsInRoleRoleAuthHandler>()
                //Register other middlewares
                .AddScoped<ErrorHandlingMiddleware>()
                //Register settings
                .Configure<BackendSettings>(_configuration.GetSection("BackendSettings"))
                .Configure<MailerSettings>(_configuration.GetSection("MailerSettings"))
                
                //Read model
                .AddScoped<IUserAccountingReadModel, UserAccountingReadModel>()
                
                //RegisterDomain
                .AddScoped<IUserManager, UserManager>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IDonationManager, DonationManager>()
                .AddScoped<INewsManager, NewsManager>()
                .AddScoped<IStoriesManager, StoriesManager>()
                .AddScoped<IDomainOptionsService, DomainOptionsService>()
                .AddScoped<IAuthentificationService, AuthentificationService>()
                .AddScoped<ITicketSender, TicketSender>()
                .AddScoped<ITicketService, TicketService>()
                .AddScoped<IUserMailerService, UserMailerService>()
                .AddScoped<IMailer, MailerImpl>()
                .AddScoped<ITicketSender, TicketSender>()
                .AddScoped<ICapitalManager, CapitalManager>()
                .AddScoped<IImageResizer, ImageResizer>()
                .AddScoped<IFileManager, FileManager>()
                .AddScoped<IPageManager, PageManager>()
                .AddScoped<ICapitalsTableHelper, CapitalsTableHelper.CapitalsTableHelper>()
                .AddScoped<INavigationHelper, NavigationHelper.NavigationHelper>()
                .AddScoped<IBannerHelper, BannerHelper.BannerHelper>();

            _services.Configure<FileStorageSettings>(_configuration.GetSection("FileStorageSettings"));
            _services.Configure<PaymentGatewaySettings>(_configuration.GetSection("PaymentGatewaySettings"));
        }

        private void ConfigureLoggly()
        {
            var settings = new LogglySettings();
            _configuration.GetSection("LogglySettings").Bind(settings);
            
            var config = LogglyConfig.Instance;
            config.CustomerToken = settings.CustomerToken;
            config.ApplicationName = $"MIPT_{settings.Location}";
            
            config.Transport.EndpointHostname = settings.EndpointHostname;
            config.Transport.EndpointPort = settings.EndpointPort;
            config.Transport.LogTransport = LogTransport.Https;

            var ct = new ApplicationNameTag {Formatter = "application-{0}"};
            config.TagConfig.Tags.Add(ct);
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


                cfg.CreateMap<CapitalUpdatingModel, Capital>()
                    .ForMember(model => model.CapitalCredentials, o => o.ResolveUsing(p => Mapper.Map<CapitalCredentials>(p.CapitalCredentials)));
                cfg.CreateMap<CapitalCreatingModel, Capital>()
                    .ForMember(model => model.CapitalCredentials, o => o.ResolveUsing(p => Mapper.Map<CapitalCredentials>(p.CapitalCredentials)));
                cfg.CreateMap<Capital, CapitalModel>();
                cfg.CreateMap<Capital, ShortCapitalModel>();
                cfg.CreateMap<Capital, CapitalModelForAdmin>()
                    .ForMember(model => model.CapitalCredentials, o => o.ResolveUsing(p => Mapper.Map<CapitalCredentialsModel>(p.CapitalCredentials)));

                cfg.CreateMap<CapitalizationModel, Capitalization>();
                cfg.CreateMap<Capitalization, CapitalizationModel>();

                cfg.CreateMap<CapitalCredentials, CapitalCredentialsModel>();
                cfg.CreateMap<CapitalCredentialsModel, CapitalCredentials>();

                cfg.CreateMap<Image, ImageModel>();
                cfg.CreateMap<ImageModel, Image>();
                
                cfg.CreateMap<PersonModel, Person>()
                    .ForMember(model => model.Image, o => o.ResolveUsing(p => Mapper.Map<Image>(p.Image)));
                cfg.CreateMap<Person, PersonModel>()
                    .ForMember(model => model.Image, o => o.ResolveUsing(p => Mapper.Map<ImageModel>(p.Image)));

                cfg.CreateMap<Page, PageModel>();
                cfg.CreateMap<PageUpdateModel, Page>();
                cfg.CreateMap<PageCreationModel, Page>();
                
                cfg.CreateMap<CapitalsTableEntry, CapitalsTableEntryModel>();
                cfg.CreateMap<CapitalsTableEntryModel, CapitalsTableEntry>();
                
                cfg.CreateMap<NavigationTableEntry, NavigationTableEntryModel>();
                cfg.CreateMap<NavigationTableEntryModel, NavigationTableEntry>();
                
                cfg.CreateMap<BannerElement, BannerElementModel>();
                cfg.CreateMap<BannerElementModel, BannerElement>();

                cfg.CreateMap<News, NewsModel>()
                    .ForMember(model => model.Image, o => o.ResolveUsing(p => Mapper.Map<ImageModel>(p.Image)))
                    .ForMember(model => model.PreviewImage, o => o.ResolveUsing(p => Mapper.Map<ImageModel>(p.PreviewImage)));
                cfg.CreateMap<News, NewsModelForAdmin>();
                cfg.CreateMap<NewsCreationModel, News>();
                
                cfg.CreateMap<Story, StoryModel>()
                    .ForMember(model => model.Owner, o => o.ResolveUsing(p => Mapper.Map<PersonModel>(p.Owner)));;
                cfg.CreateMap<StoryCreationModel, Story>();
                cfg.CreateMap<StoryCreationModel, Story>();

                cfg.CreateMap<DonationWithRegistrationModel, CreateDonationModel>();
                cfg.CreateMap<DonationWithRegistrationModel, User>();
                cfg.CreateMap<Donation, ShortDonationModel>();
                cfg.CreateMap<CreateDonationModel, Donation>();
                cfg.CreateMap<Donation, DonationRelatedToUserModdel>();
            });
            
        }

        private void ConfigureDatebase()
        {
            var connectionString = _configuration.GetConnectionString("Postgres");
            
            _services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<DomainOptionsContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<UserContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<WithImageContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<PageContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<TicketContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<NavigationTableContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<BannerContext>(options => options.UseNpgsql(connectionString))
                .AddDbContext<DonationContext>(options => options.UseNpgsql(connectionString))

                .AddScoped<IGenericRepository<User>, UserRepository>()
                .AddScoped<IGenericRepository<Capital>, CapitalRepository>()
                .AddScoped<IGenericRepository<News>, NewsRepository>()
                .AddScoped<IGenericRepository<Story>, StoriesRepository>()
                .AddScoped<IGenericRepository<Donation>, DonationRepository>()
                .AddScoped<ICapitalsTableEntryRepository, CapitalsTableRepository>()
                .AddScoped<INavigationTableRepository, NavigationTableRepository>()
                .AddScoped<IBannerRepository, BannerRepository>()
                .AddScoped<ICapitalRepository, CapitalRepository>()
                .AddScoped<IDomainOptionsRepository, DomainOptionsRepository>()
                .AddScoped<IPageRepository, PageRepository>()
                .AddScoped<ITicketRepository, TicketRepository>()
                
                .AddScoped<IPaymentProvider, PaymentProvider>();
        }
    }
}