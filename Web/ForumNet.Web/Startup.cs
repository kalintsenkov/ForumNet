namespace ForumNet.Web
{
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Data;
    using Data.Models;
    using Data.Seeding;
    using Services;
    using Services.Contracts;
    using Services.Messaging;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ForumDbContext>(options =>
                options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services
                .AddDefaultIdentity<ForumUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ForumRole>()
                .AddEntityFrameworkStores<ForumDbContext>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = this.configuration["Facebook:AppId"];
                    facebookOptions.AppSecret = this.configuration["Facebook:AppSecret"];
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = this.configuration["Google:ClientId"];
                    googleOptions.ClientSecret = this.configuration["Google:ClientSecret"];
                });

            services.AddAutoMapper(typeof(ForumNetProfile).Assembly);

            services.AddSingleton(this.configuration);

            services.AddTransient<IEmailSender>(
                serviceProvider => new SendGridEmailSender(this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<IPostReactionsService, PostReactionsService>();
            services.AddTransient<IPostReportsService, PostReportsService>();
            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<IRepliesService, RepliesService>();
            services.AddTransient<IReplyReactionsService, ReplyReactionsService>();
            services.AddTransient<IReplyReportsService, ReplyReportsService>();
            services.AddTransient<ITagsService, TagsService>();
            services.AddTransient<IUsersService, UsersService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            using var dbContext = serviceScope.ServiceProvider.GetRequiredService<ForumDbContext>();

            if (env.IsDevelopment())
            {
                dbContext.Database.Migrate();
            }

            new ForumDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("/Home/NotFound{0}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Posts}/{action=Trending}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
