﻿namespace ForumNet.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Common;
    using Data;
    using Data.Models;
    using Services.Categories;
    using Services.Messages;
    using Services.Posts;
    using Services.Providers.DateTime;
    using Services.Providers.Email;
    using Services.Reactions;
    using Services.Replies;
    using Services.Reports;
    using Services.Tags;
    using Services.Users;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddDbContext<ForumDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddDefaultIdentity<ForumUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = GlobalConstants.UserPasswordMinLength;
                    options.User.RequireUniqueEmail = true;
                    //options.SignIn.RequireConfirmedAccount = true;
                    //options.SignIn.RequireConfirmedEmail = true;
                })
                .AddRoles<ForumRole>()
                .AddEntityFrameworkStores<ForumDbContext>();

            return services;
        }

        public static IServiceCollection ConfigureCookiePolicyOptions(this IServiceCollection services)
            => services
                .Configure<CookiePolicyOptions>(options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

        public static IServiceCollection AddResponseCompressionForHttps(this IServiceCollection services)
            => services
                .AddResponseCompression(options => options
                    .EnableForHttps = true);

        public static IServiceCollection AddAntiforgeryHeader(this IServiceCollection services)
            => services
                .AddAntiforgery(options => options
                    .HeaderName = "X-CSRF-TOKEN");

        public static IServiceCollection AddFacebookAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = configuration["Facebook:AppId"];
                    facebookOptions.AppSecret = configuration["Facebook:AppSecret"];
                });

            return services;
        }

        public static IServiceCollection AddGoogleAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = configuration["Google:ClientId"];
                    googleOptions.ClientSecret = configuration["Google:ClientSecret"];
                });

            return services;
        }

        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddSingleton(configuration)
                .AddTransient<ICategoriesService, CategoriesService>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>()
                .AddTransient<IMessagesService, MessagesService>()
                .AddTransient<IPostReactionsService, PostReactionsService>()
                .AddTransient<IPostReportsService, PostReportsService>()
                .AddTransient<IPostsService, PostsService>()
                .AddTransient<IRepliesService, RepliesService>()
                .AddTransient<IReplyReactionsService, ReplyReactionsService>()
                .AddTransient<IReplyReportsService, ReplyReportsService>()
                .AddTransient<ITagsService, TagsService>()
                .AddTransient<IUsersService, UsersService>()
                .AddTransient<IEmailSender>(serviceProvider => 
                    new SendGridEmailSender(configuration["SendGrid:ApiKey"]));

        public static IServiceCollection AddControllersWithAntiforgeryTokenAttribute(this IServiceCollection services)
        {
            services
                .AddControllersWithViews(options => options
                    .Filters
                    .Add<AutoValidateAntiforgeryTokenAttribute>());

            return services;
        }
    }
}
