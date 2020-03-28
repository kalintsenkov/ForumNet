namespace ForumNet.Web.Tests.Controllers
{
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using ViewModels;
    using Web.Controllers;

    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldRedirectToPostControllerAllAction()
            => MyController<HomeController>
                .Calling(c => c.Index())
                .ShouldReturn()
                .RedirectToAction("All", "Posts");

        [Fact]
        public void PrivacyShouldReturnViewWithDefaultName()
            => MyController<HomeController>
                .Calling(c => c.Privacy())
                .ShouldReturn()
                .View(v => v.WithDefaultName());

        [Fact]
        public void NotFound404ShouldReturnViewWithDefaultName()
            => MyController<HomeController>
                .Calling(c => c.NotFound404())
                .ShouldReturn()
                .View(v => v.WithDefaultName());

        [Fact]
        public void ErrorShouldReturnViewWithCorrectViewModel()
            => MyController<HomeController>
                .Calling(c => c.Error())
                .ShouldReturn()
                .View(v => v
                    .WithDefaultName()
                    .WithModelOfType<ErrorViewModel>());
    }
}
