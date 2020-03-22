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

        [Theory]
        [InlineData(1, "Test 1")]
        [InlineData(2, "Test 2")]
        [InlineData(3, "Test 3")]
        public void DetailsShouldReturnCorrectView(int id, string name)
            => MyController<CategoriesController>
                .Instance()
                .WithData(new Category
                {
                    Id = id,
                    Name = name,
                    CreatedOn = DateTime.UtcNow
                })
                .Calling(c => c.Details(id, "recent"))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CategoriesDetailsViewModel>()
                    .Passing(model =>
                    {
                        model.Category.Id = id;
                        model.Category.Name = name;
                    }));
    }
}