namespace ForumNet.Web.Tests.Controllers
{
    using System;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using Data.Models;
    using ViewModels.Categories;
    using Web.Controllers;

    public class CategoriesControllerTest
    {
        [Fact]
        public void DetailsShouldReturnNotFoundWhenArticleIdIsInvalid()
            => MyController<CategoriesController>
                .Instance()
                .WithUser()
                .WithData(new Category
                {
                    Id = 1,
                    Name = "Test category",
                    CreatedOn = DateTime.UtcNow
                })
                .Calling(c => c.Details(2, "recent"))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void DetailsShouldHaveRestrictionsForAuthorizedUsers()
            => MyController<CategoriesController>
                .Calling(c => c.Details(1, ""))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests());
    }
}