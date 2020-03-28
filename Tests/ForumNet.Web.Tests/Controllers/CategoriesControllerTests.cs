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

        [Fact]
        public void DetailsShouldReturnCorrectViewModel()
           => MyController<CategoriesController>
               .Instance()
               .WithUser()
               .WithData(entities => entities
                   .WithSet<Category>(set => set
                       .Add(new Category
                       {
                           Id = 1,
                           Name = "Test category",
                           CreatedOn = DateTime.UtcNow
                       }))
                   .WithSet<Tag>(set => set
                       .Add(new Tag
                       {
                           Id = 1,
                           Name = "Test tag",
                           CreatedOn = DateTime.UtcNow
                       }))
                   .WithSet<Post>(set => set
                       .Add(new Post
                       {
                           Id = 1,
                           Title = "Test title",
                           Description = "Test description",
                           AuthorId = TestUser.Identifier,
                           CategoryId = 1,
                           CreatedOn = DateTime.UtcNow
                       }))
                   .WithSet<PostTag>(set => set
                       .Add(new PostTag
                       {
                           PostId = 1,
                           TagId = 1
                       })))
               .Calling(c => c.Details(1, null))
               .ShouldReturn()
               .View();
    }
}