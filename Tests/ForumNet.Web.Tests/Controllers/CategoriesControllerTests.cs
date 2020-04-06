namespace ForumNet.Web.Tests.Controllers
{
    using System;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using Data.Models;
    using ViewModels.Categories;
    using Web.Controllers;

    public class CategoriesControllerTests
    {
        [Fact]
        public void AllShouldReturnCorrectViewModel()
            => MyController<CategoriesController>
                .Instance()
                .WithUser()
                .Calling(c => c.All("recent"))
                .ShouldReturn()
                .View(v => v
                    .WithModelOfType<CategoriesAllViewModel>()
                    .Passing(c => c.Search == "recent"));

        [Fact]
        public void DetailsShouldReturnNotFoundWhenArticleIdIsInvalid()
            => MyController<CategoriesController>
                .Instance()
                .WithUser()
                .WithData(entities => entities
                    .WithSet<Category>(set => set
                        .Add(new Category
                        {
                            Id = 1,
                            Name = "Test",
                            CreatedOn = DateTime.UtcNow
                        })))
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