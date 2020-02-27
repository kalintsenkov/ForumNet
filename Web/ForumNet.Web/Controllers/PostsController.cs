namespace ForumNet.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Posts;

    public class PostsController : Controller
    {
        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostsInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var authorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.postsService.CreateAsync(input.Title, input.Description, authorId, input.CategoryId);

            return RedirectToAction("Index", "Home");
        }

        //// GET: Posts/Details/5
        //public IActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Posts/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// GET: Posts/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}
    }
}