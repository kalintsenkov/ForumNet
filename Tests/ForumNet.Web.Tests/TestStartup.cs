namespace ForumNet.Web.Tests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Mocks;
    using MyTested.AspNetCore.Mvc;

    using Services.Contracts;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) 
            : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            // services.ReplaceTransient<ICategoriesService>(_ => CategoriesServiceMock.Instance);
            // services.ReplaceTransient<IPostsService>(_ => PostsServiceMock.Instance);
            // services.ReplaceTransient<ITagsService>(_ => TagsServiceMock.Instance);
        }
    }
}
